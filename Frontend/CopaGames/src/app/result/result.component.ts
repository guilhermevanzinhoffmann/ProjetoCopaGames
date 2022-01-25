import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Game } from '../models/game';

@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.scss']
})
export class ResultComponent implements OnInit {

  title = 'resultado final';
  winnerGames: Array<Game> = new Array<Game>();

  constructor(private router: Router) {
    let nav = this.router.getCurrentNavigation();
    if(nav?.extras?.state?.games != null)
      this.winnerGames = nav.extras.state.games;
  }

  ngOnInit(): void {
    if(this.winnerGames.length == 0)
      this.router.navigateByUrl('/selection');
  }

  ngOnDestroy(): void {
    this.winnerGames = new Array<Game>();
  }

  goToSelection(): void{
    this.router.navigateByUrl('/selection');
  }
}
