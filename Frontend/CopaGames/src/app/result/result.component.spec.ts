import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatGridListModule } from '@angular/material/grid-list';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { AppRoutingModule } from '../app-routing.module';
import { Game } from '../models/game';
import { ResultComponent } from './result.component';

describe('ResultComponent', () => {
  let component: ResultComponent;
  let fixture: ComponentFixture<ResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        HttpClientModule,
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        MatCardModule,
        MatButtonModule,
        MatGridListModule,
        MatCheckboxModule,
        FormsModule
      ],

      declarations: [ ResultComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('deve criar', () => {
    expect(component).toBeTruthy();
  });

  it('deve ter titulo correto', () => {
    const title = fixture.nativeElement.querySelector('#title');
    expect(component.title).toBe('resultado final');
    expect(title.textContent).toEqual('RESULTADO FINAL');
  });

  it('deve aparecer titulo correto do game', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1'), new Game('2', 'game 2 (NTST)', 2, 2, 'url2')];
    component.winnerGames = games;
    fixture.detectChanges();
    const title = fixture.nativeElement.querySelector('#title-game');
    expect(title.textContent).toEqual('game 1 ');
  });

  it('deve aparecer ano correto', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1'), new Game('2', 'game 2 (NTST)', 2, 2, 'url2')];
    component.winnerGames = games;
    fixture.detectChanges();
    const title = fixture.nativeElement.querySelector('#year');
    expect(title.textContent[0]).toEqual(games[0].ano.toString());
  });


  it('deve aparecer posicao correta do game', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1'), new Game('2', 'game 2 (NTST)', 2, 2, 'url2')];
    component.winnerGames = games;
    fixture.detectChanges();
    const posicao = fixture.nativeElement.querySelector('#position');
    expect(posicao.textContent).toEqual('1º Lugar');
  })

  it('deve aparecer console correto', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1'), new Game('2', 'game 2 (NTST)', 2, 2, 'url2')];
    component.winnerGames = games;
    fixture.detectChanges();
    const title = fixture.nativeElement.querySelector('#console');
    expect(title.textContent).toEqual('TST');
  });

  it('deve carregar url correta', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1'), new Game('2', 'game 2 (NTST)', 2, 2, 'url2')];
    component.winnerGames = games;
    fixture.detectChanges();
    const title = fixture.nativeElement.querySelector('#urlImage');
    expect(title.getAttribute('src')).toEqual('url1');
  });
});
