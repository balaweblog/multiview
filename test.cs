export function notfound(res) {
    res.status(404).json({response: "error", message:"no record found"});
}
export function ok(res) {
    res.status(201).json({response: "success", message:"record inserted/updated."});
}
export function error(res) {
    res.status(500).json({response: "error", message:"server error"})
}
export function invalidrequest(res) {
    res.status(400).json({response: "error", message:"invalid http request"})
}
export function invalidbody(res) {
    res.status(400).json({response: "error", message:"invalid message body"})
}
export function out(res,output) {
    res.status(200).json(output);
}
