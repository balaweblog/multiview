import * as express from 'express';
import * as Session from 'express-session';

import { getcountries, getstates, getcitiesbystateid, getjoblocation, getskillset} from '../controller/utility/utilitycontroller';
import { createEmployer } from '../controller/employer/employercontroller';
import { createJob, getjobs, getjobsbyJobId, getjobsbydate, updatejobStatus } from '../controller/job/jobcontroller';
import { googleredirect, facebookredirect, facebooklogin, googlelogin, twitterlogin, twitterredirect, localsignup, locallogin } from '../controller/auth/authcontroller';
import { error, success, loginsuccess, verifywebtoken , encryption, decryption } from '../controller/account/accountcontroller';
import { userProfile, getuserprofilebyEmail, hasuserProfile, getuserProfilebyLastWorkingday, updateuserProfile, getreferencestatus, updatereferencestatus, getuserprofilebyreference, getprofilereferencestatusbyemail } from '../controller/candidate/candidatecontroller';
import { getNotification, updateNotification, insertNotification } from '../controller/utility/notificationcontroller';
import { updatejobstatus, applyjob, getappliedjobs, deleteappliedjob, deleteappliedjobbyJobId, getallappliedjobs, hasappliedjobs } from '../controller/job/applyjobcontroller';

var passport = require('passport');
const SchemaValidator = require('../validation/schemavalidator');


export default (app) => {

    const apiRoutes = express.Router();
    const utitlityroutes = express.Router();
    const coreroutes = express.Router();
    const accountroutes = express.Router();
    const authroutes = express.Router();

    // Session secret only server side.
    app.use(Session({
        secret: 'raysources-secret-19890913007',
        resave: true,
        saveUninitialized: true
    }));

    // passport and session 
    app.use(passport.initialize());
    app.use(passport.session());

    //Joi Validation 
    const validateRequest = SchemaValidator(false);


    // common route
    apiRoutes.use('/utilities',verifywebtoken,utitlityroutes);
    apiRoutes.use('/core',coreroutes);
    apiRoutes.use('/account',accountroutes);
    apiRoutes.use('/auth',authroutes);

    //utility controller routes 
    // notification
    utitlityroutes.put('/notification/:id',updateNotification);
    utitlityroutes.get('/notification/:status',getNotification);
    utitlityroutes.post('/notification/:emailaddress',insertNotification);
    //utility
    utitlityroutes.get('/countries', getcountries);
    utitlityroutes.get('/states', getstates);
    utitlityroutes.get('/cities/:id',getcitiesbystateid);
    utitlityroutes.get('/joblocations',getjoblocation);
    utitlityroutes.get('/skillset',getskillset);

    //core controller routes
    // job
    coreroutes.post('/job',createJob);
    coreroutes.get('/job/:title/:emailaddress/:experience/:minsalary/:maxsalary/:location',getjobs);
    coreroutes.get('/job/:jobid',getjobsbyJobId);
    coreroutes.get('/jobs/:currentdate',getjobsbydate);
    coreroutes.put('/job/:jobid',updatejobStatus);
    // applied job
    coreroutes.post('/appliedjob',applyjob);
    coreroutes.get('/appliedjob/:emailaddress',getappliedjobs);
    coreroutes.delete('/appliedjob/:jobid',deleteappliedjob);
    coreroutes.delete('/appliedjobs/:jobid',deleteappliedjobbyJobId);
    coreroutes.get('/appliedjobbystatus/:jobstatus',getallappliedjobs);
    coreroutes.put('/appliedjob/:emailaddress',updatejobstatus);
    coreroutes.get('/appliedjobbyemail/:emailaddress',hasappliedjobs);
    // employer
    coreroutes.post('/employer',createEmployer);
    // user profile
    coreroutes.post('/userprofile',validateRequest ,userProfile);
    coreroutes.put('/userprofile/:emailaddress',updateuserProfile);
    coreroutes.put('/userprofilebyreferencestatus/:emailaddress',updatereferencestatus);
    coreroutes.get('/userprofilebyreferences/:referencestatus',getuserprofilebyreference);
    coreroutes.get('/userprofilebyreferencestatus/:emailaddress/:referencename/:referencenumber',
    getreferencestatus);
    coreroutes.get('/userprofilebyreferencestatusemail/:emailaddress', 
    getprofilereferencestatusbyemail);
    coreroutes.get('/userprofilebyemail/:emailaddress',getuserprofilebyEmail);
    coreroutes.get('/userprofilebylwd/:days',getuserProfilebyLastWorkingday);
    coreroutes.get('/userprofile/:emailaddress',hasuserProfile);

    //auth routes 
    authroutes.get('/facebook',facebooklogin);
    authroutes.get('/facebookoauthcallback',facebookredirect);
    authroutes.get('/google', googlelogin);
    authroutes.get('/googleoauthcallback',googleredirect);
    authroutes.get('/twitter',twitterlogin);
    authroutes.get('/twitteroauthcallback',twitterredirect);
    authroutes.post('/localsignup',localsignup);
    authroutes.post('/locallogin',locallogin);
    
    //account routes
    accountroutes.get('/error',error);
    accountroutes.get('/success',success);
    accountroutes.get('/loginsuccess',loginsuccess);
    accountroutes.get('/encrypt/:message',encryption);
    accountroutes.get('/decrypt/:message',decryption);

    // append route to app 
    app.use('/api',apiRoutes);
}
