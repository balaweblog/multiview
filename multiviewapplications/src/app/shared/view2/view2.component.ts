import { Component, OnInit } from '@angular/core';
import { RootService } from '../../service/root.service';

@Component({
  selector: 'app-view2',
  templateUrl: './view2.component.html',
  styleUrls: ['./view2.component.css']
})
export class View2Component implements OnInit {

  constructor(private rootsevice: RootService) { }
  people: any[];

  ngOnInit() {
    this.rootsevice.getpeople().then(
      res1 => { this.people = res1; }
    );
  }

}
