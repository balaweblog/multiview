import config from './main';
var winston = require('winston');

var logger = winston.createLogger({
    transports: [
        new winston.transports.File({
            level: config.log.level,
            filename: config.log.path,
            handleExceptions: true,
            json: true,
            maxsize: 5242880, //5MB
            maxFiles: 5,
            colorize: false
        }),
    ],
    exitOnError: false
});

module.exports = logger;
module.exports.stream = {
    write: function(message, encoding){
        logger.info(message);
    }
};
