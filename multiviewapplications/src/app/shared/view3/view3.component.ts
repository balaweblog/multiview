import { Component, OnInit } from '@angular/core';
import { RootService } from '../../service/root.service';

@Component({
  selector: 'app-view3',
  templateUrl: './view3.component.html',
  styleUrls: ['./view3.component.css']
})
export class View3Component implements OnInit {

  constructor(private rootsevice: RootService) { }
  locations: any[];

  ngOnInit() {
    this.rootsevice.getlocations().then(
      res1 => { this.locations = res1; }
    );
  }

}
