import { MatGridListModule } from '@angular/material/grid-list';
import { Game } from './../models/game';
import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, getTestBed, TestBed } from '@angular/core/testing';
import { SelectionComponent } from './selection.component';
import { MatCardModule } from '@angular/material/card';
import { BrowserModule, By } from '@angular/platform-browser';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { ResultComponent } from '../result/result.component';

describe('SelectionComponent', () => {
  let component: SelectionComponent;
  let fixture: ComponentFixture<SelectionComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
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
      declarations: [ SelectionComponent, ResultComponent]
    })
    .compileComponents();

  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('deve criar', () => {
    expect(component).toBeTruthy();
  });

  it('deve ter titulo correto', () => {
    const title = fixture.nativeElement.querySelector('#title');
    expect(component.title).toBe('fase de seleção');
    expect(title.textContent).toEqual('FASE DE SELEÇÃO');
  });

  it('deve aparecer titulo correto do game', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1'), new Game('2', 'game 2 (NTST)', 2, 2, 'url2')];
    component.games = games;
    fixture.detectChanges();
    const title = fixture.nativeElement.querySelector('#title-game');
    expect(title.textContent).toEqual('game 1 ');
  });

  it('deve aparecer console correto', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1'), new Game('2', 'game 2 (NTST)', 2, 2, 'url2')];
    component.games = games;
    fixture.detectChanges();
    const title = fixture.nativeElement.querySelector('#console');
    expect(title.textContent).toEqual('TST');
  });

  it('deve aparecer ano correto', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1'), new Game('2', 'game 2 (NTST)', 2, 2, 'url2')];
    component.games = games;
    fixture.detectChanges();
    const title = fixture.nativeElement.querySelector('#year');
    expect(title.textContent[0]).toEqual(games[0].ano.toString());
  });

  it('deve carregar url correta', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1'), new Game('2', 'game 2 (NTST)', 2, 2, 'url2')];
    component.games = games;
    fixture.detectChanges();
    const title = fixture.nativeElement.querySelector('#urlImage');
    expect(title.getAttribute('src')).toEqual('url1');
  });

  it('deve selecionar game', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1')];
    component.games = games;
    fixture.detectChanges();
    const checkbox = fixture.nativeElement.querySelector('#title-game');
    checkbox.click();
    fixture.detectChanges();
    expect(component.games[0].selected).toBeTrue();
  });

  it('deve desmarcar game', () => {
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1')];
    component.games = games;
    fixture.detectChanges();
    const checkbox = fixture.nativeElement.querySelector('#title-game');
    checkbox.click();
    fixture.detectChanges();
    checkbox.click();
    fixture.detectChanges();
    expect(!component.games[0].selected).toBeTrue();
  });

  it('checkbox deve estar habilitado', () =>{
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1')];
    component.games = games;
    fixture.detectChanges();
    const checkbox = fixture.nativeElement.querySelector('#checkbox0-input');
    expect(!checkbox.disabled).toBeTrue();
  });

  it('checkbox deve estar desabilitado', () =>{
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1')];
    component.games = games;
    component.totalSelected = 8;
    fixture.detectChanges();
    const checkbox = fixture.nativeElement.querySelector('#checkbox0-input');
    expect(checkbox.disabled).toBeTrue();
  });

  it('checkbox marcado deve estar habilitado', () =>{
    const games = [new Game('1', 'game 1 (TST)', 1, 1, 'url1')];
    component.games = games;
    fixture.detectChanges();
    const checkboxInput = fixture.nativeElement.querySelector('#title-game');
    checkboxInput.click();
    component.totalSelected = 8;
    fixture.detectChanges();
    const checkbox = fixture.nativeElement.querySelector('#checkbox0-input');
    expect(!checkbox.disabled).toBeTrue();
  });

  it('botao deve estar habilitado', () =>{
    component.totalSelected = 8;
    fixture.detectChanges();
    const button = fixture.nativeElement.querySelector('#button');
    expect(!button.disabled).toBeTrue();
  });

  it('botao deve estar desabilitado', () =>{
    component.totalSelected = 7;
    fixture.detectChanges();
    const button = fixture.nativeElement.querySelector('#button');
    expect(button.disabled).toBeTrue();
  });
});
