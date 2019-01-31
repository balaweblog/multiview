import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { View1Component } from './shared/view1/view1.component';
import { View2Component } from './shared/view2/view2.component';
import { View3Component } from './shared/view3/view3.component';
import { RootService } from './service/root.service';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    View1Component,
    View2Component,
    View3Component
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [RootService],
  entryComponents: [
    View1Component,
    View2Component,
    View3Component
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
