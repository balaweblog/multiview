const  nodemon = require("gulp-nodemon");
const ts = require('gulp-typescript');
const gulp = require('gulp');
const del = require("del");
const pm2 = require('pm2');


    // pull in the project Typescript config
    const tsProject = ts.createProject('tsconfig.json');

     
    // copy the certificate folders
    gulp.task('copy certificate',['clean task '] ,function() {
      gulp.src('./cert/**')
      .pipe(gulp.dest('build/cert'));
   });

   // create log folder
   gulp.task('create log folder',['clean task '],function() {
    gulp.src('*.*',{read:false})
    .pipe(gulp.dest('build/log'));
   });

   // delete build folder
   gulp.task('clean task ', function(){
      return del('build/**', {force:true});
   });

    // task to be run when the watcher detects changes
    gulp.task('build',['clean task ','copy certificate','create log folder'], () => {
      const tsResult = tsProject.src()
      .pipe(tsProject());
      return tsResult.js.pipe(gulp.dest('build'));
    });

    // gulp run in prod mode. 
    gulp.task('deploy', function () {
        nodemon({ script: 'build/server.js' })
          .on('restart', function () {
            console.log('restarted!')
          })
    });
    // pm2 production stop 
    gulp.task('prodstart', function () {
      pm2.connect(true, function () {
          pm2.start({
              name: 'hotcakeapi',
              script: 'build/server.js',
              env: {

              }
          }, function () {
              console.log('pm2 hotcakeapi started');
              pm2.streamLogs('all', 0);
          });
      });
  });
  // pm2 production stop 
  gulp.task('prodstop', function () {
        pm2.connect(true, function () {
            pm2.stop("hotcakeapi", function() {
              console.log('pm2 hotcakeapi stopped');
              pm2.streamLogs('all', 0);
            });
        });
    });
  // pm2 production restart 
  gulp.task('prodrestart', function () {
      pm2.connect(true, function () {
          pm2.restart("hotcakeapi", function() {
            console.log('pm2 hotcakeapi restarted');
            pm2.streamLogs('all', 0);
          });
      });
  });
  // pm2 production delete 
  gulp.task('proddelete', function () {
    pm2.connect(true, function () {
        pm2.delete("hotcakeapi", function() {
          console.log('pm2 hotcakeapi deleted');
          pm2.streamLogs('all', 0);
        });
    });
  });
  
    // default task  for watcher
    gulp.task('default', ['watch']); 

export {};
