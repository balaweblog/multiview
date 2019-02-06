  userprofile.findOneAndUpdate(query, update, {upsert:true}).then(
        item => {
            if( item ==null) {
                notfound(res);
            } else {
                insertnotificationtable(email,"You Profile Deleted");
                ok(res);
            }
        }).catch(_ => error(res));

}
