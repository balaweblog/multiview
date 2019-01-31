import { Component, ComponentFactoryResolver, ViewChild, ViewContainerRef, ComponentRef, ComponentFactory } from '@angular/core';
import { View1Component } from './shared/view1/view1.component';
import { View2Component } from './shared/view2/view2.component';
import { View3Component } from './shared/view3/view3.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  @ViewChild('contentplaceholder', { read: ViewContainerRef }) container;

  componentRef: ComponentRef<any>;
  title = 'multiviewapplications';
  factory: ComponentFactory<any>;

  constructor(private resolver: ComponentFactoryResolver) {
  }

  ngOnInit() {
    // default component registration
    this.factory = this.resolver.resolveComponentFactory(View1Component);
    this.componentRef = this.container.createComponent(this.factory);
  }
  createComponent(type) {
    this.container.clear();

    if (type === 'view1') {
       this.factory = this.resolver.resolveComponentFactory(View1Component);
    } else if (type === 'view2') {
        this.factory = this.resolver.resolveComponentFactory(View2Component);
    } else if (type === 'view3') {
      this.factory = this.resolver.resolveComponentFactory(View3Component);
    }

    this.componentRef = this.container.createComponent(this.factory);
    this.componentRef.instance.type = type;
  }
}
