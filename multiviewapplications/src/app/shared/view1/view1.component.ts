import { Component, OnInit } from '@angular/core';
import { RootService } from '../../service/root.service';

@Component({
  selector: 'app-view1',
  templateUrl: './view1.component.html',
  styleUrls: ['./view1.component.css']
})
export class View1Component implements OnInit {

  constructor(private rootsevice: RootService) { }
  films: any[];

  ngOnInit() {
    this.rootsevice.getfilms().then(
      res1 => { this.films = res1; }
    );
  }

}
