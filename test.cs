'use strict';
var express = require('express');
var mongoose = require('mongoose');
var bodyParser = require('body-parser');
var cors = require('cors');
var fs = require('fs')
var morgan = require('morgan')
var https = require('https');
var path = require('path')
var rfs = require('rotating-file-stream')
const helmet = require('helmet');
const expressOasGenerator = require('express-oas-generator');
var compression = require('compression')
var RateLimit = require('express-rate-limit');
const logger = require('./config/log')


import router from './router/router';
import config from './config/main';

// init express
const app = express();

var limiter = new RateLimit({
    windowMs: 60*60*1000, // 15 minutes 
    max: 10000, // limit each IP to 100 requests per windowMs 
    delayMs: 0 // disable delaying - full speed until the max limit is reached 
});

// rate limiter for all requests
app.use(limiter);
  

// swagger generator /api-docs
expressOasGenerator.init(app, {});


// https options
var certDirectory = path.join(__dirname, 'cert')
var httpsOptions = {
key: fs.readFileSync(path.join(certDirectory, 'key.pem'),'utf8'),
cert: fs.readFileSync(path.join(certDirectory, 'server.crt'),'utf8')
};

// enable log
app.use(morgan('combined', {stream: logger.stream}))

// enable compression to reduce the size of response body
app.use(compression())


// init mongoose
mongoose.connect(config.mongodb.database);

// middleware
app.use(bodyParser.urlencoded({extended:true}));
app.use(bodyParser.json());
app.disable('etag');

// cors enable
app.use(cors())

// secure http headers
app.use(helmet({frameguard: {action: 'deny'}}))
app.disable('x-powered-by')

// router
router(app);

//mongoose.set('debug', true);

// init server
let server;
server = https.createServer(httpsOptions, app).listen(config.server.port,function(){
    console.log(`node server listening on ${config.server.port}`);
});

// export
export default server;
