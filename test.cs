import * as mongoose from 'mongoose';
import { ObjectId } from 'mongodb';
import { invalidbody, ok, error, invalidrequest, out, notfound } from '../account/accountcontroller';
import job from '../../models/job';
import appliedjob from '../../models/appliedjob';


/* create new job */
export function createJob(req,res){
    if (Object.keys(req.body).length === 0) {
        invalidbody(res);
        return;
    }
    const jobs = new job(req.body);
    jobs.save().then(e => {ok(res)}).catch(_ => error(res));
}

/* get all jobs matching the search criteria */
export function getjobs(req,res) {
    const title = req.params["title"];
    const location = req.params['location'];
    const emailaddress = req.params['emailaddress'];
    const experience = Number(req.params['experience']);
    const minsalary = Number(req.params['minsalary']);
    const maxsalary = Number(req.params['maxsalary']);
    let jobinformation: string[] =[];
    
    if(title === "" || location === "" || emailaddress === "" || experience < 1 || minsalary < 1 || maxsalary < 1 ){
        invalidrequest(res);
        return;
    }

    // if any job is already applied
    appliedjob.find({emailaddress:emailaddress},(_, appliedjobsdetails)=>{
        appliedjobsdetails.forEach(function(_, doc) {
            jobinformation.push(appliedjobsdetails[doc]["jobid"]);
    });

    job.find({   
        $or: [
            { $and: [{ minexperience: {$lte:experience}, maxexperience: {$gte:experience}}]},
            { $and: [{ minexperience: {$lte:experience-1}, maxexperience: {$gte:experience-1}}]},
            { $and: [{ minexperience: {$lte:experience+1}, maxexperience: {$gte:experience+1}}]}
        ],
        $and: [
            { $and: [{maxsalary: {$gte: minsalary-1, $lte: maxsalary+1}}]},
        ],
        location: {
            $regex: location.split(',').join('|'),
            $options: "i"
        },
        skillset: {
            $regex: title.split(',').join('|'),
            $options: "i"
        },
        _id: { "$nin": jobinformation}}).then(item => 
            {
                if (item.length <1) {
                    notfound(res);
                } else {
                    out(res,item)
                }
            }).catch(_ => error(res));
    });
}
/* get jobs by job Id */
export function getjobsbyJobId(req,res ){
    const jobid = req.params["jobid"];
    const jobidarr = jobid.split(',')
    for (let i =0; i< jobidarr.length; i++){
        if (!ObjectId.isValid(jobidarr[i])) {
            invalidrequest(res);
            return
        }
    }
    job.find({_id: {$in: jobidarr.map(function(o){ return mongoose.Types.ObjectId(o); })}}).then(item => 
        {
            if (item.length<1) {
                notfound(res);
            } else {
                out(res,item)
            }
        }).catch(_ => error(res));
}
/* get jobs by date */
export function getjobsbydate(req,res,next) {

    if(Date.parse(req.params['currentdate'])<1) {
        invalidrequest(res);
        return;
    }
    const currentdate = new Date(req.params['currentdate']);
    const currentdate1 = new Date(req.params['currentdate']);
    var start = currentdate;
    start.setHours(0,0,0,0);
    var end = currentdate1;
    end.setHours(23,59,59,999);
    var query = {status:"Active",enddate: {$gte: start, $lte: end}};

    job.find(query).then(item => 
        {
            if (item.length < 1) {
                notfound(res);
            } else {
                out(res,item)
            }
        }).catch(_ => error(res));
}
/* update job status */
export function updatejobStatus(req,res) {
    const id = req.params.jobid;

    if (!ObjectId.isValid(id)) {
        invalidrequest(res);
        return;
    }

    job.findOneAndUpdate({_id:id}, {status:"InActive"}, {upsert:true}).then(
        item => {
            if (item ==null) {
                notfound(res);
            } else {
                ok(res);
            }
        }).catch(_ => error(res));

}
