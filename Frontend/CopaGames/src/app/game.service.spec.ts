import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { GameService } from './game.service';
import { Game } from './models/game';

describe('GameService', () => {
  let service: GameService;
  let httpClient : HttpClient;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [GameService, HttpClient]
    });

    service = TestBed.inject(GameService);
    httpClient = TestBed.inject(HttpClient);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('deve ser criado', () => {
    expect(service).toBeTruthy();
  });

  it('deve chamar getGames() com o endpoint correto', () => {
    const spy = spyOn(httpClient, 'get').and.callThrough();
    service.getGames();
    expect(spy).toHaveBeenCalledWith('https://localhost:7057/api/v1/Game/getAll');
  });

  it('deve chamar getResult() com o endpoint correto', () => {
    const listGames: Game[] = [
      new Game("1", "game1", 1, 1, "url1"),
      new Game("2", "game2", 2, 2, "url2")
    ];

    const spy = spyOn(httpClient, 'post').and.callThrough();
    service.getResult(listGames);
    expect(spy).toHaveBeenCalledWith('https://localhost:7057/api/v1/Game/result', listGames);
  });

  it('deve chamar getGames() e retornar um Observable<Game[]>', () => {
    const listGames: Game[] = [
      new Game("1", "game1", 1, 1, "url1"),
      new Game("2", "game2", 2, 2, "url2")
    ];

    service.getGames().subscribe(games => {
      expect(games.length).toBe(2);
      expect(games).toEqual(listGames);
    });

    const req = httpMock.expectOne('https://localhost:7057/api/v1/Game/getAll');
    expect(req.request.method).toBe("GET");
    req.flush(listGames);
  });

  it('deve chamar getResult() e retornar um Observable<Game[]>', () => {
    const listGames: Game[] = [
      new Game("1", "game1", 1, 1, "url1"),
      new Game("2", "game2", 2, 2, "url2")
    ];

    const listEntryGames: Game[] = [
      new Game("1", "game1", 1, 1, "url1"),
      new Game("2", "game2", 2, 2, "url2")
    ];

    service.getResult(listEntryGames).subscribe(games => {
      expect(games.length).toBe(2);
      expect(games).toEqual(listGames);
    });

    const req = httpMock.expectOne('https://localhost:7057/api/v1/Game/result');
    expect(req.request.method).toBe("POST");
    req.flush(listGames);
  });

  it('deve chamar getGames() e retornar erro 404', () => {
    service.getGames().subscribe(games => {
      expect(games.length).toBe(0);
    }, error => {
      expect(error.status).toBe(404);
    });

    const req = httpMock.expectOne('https://localhost:7057/api/v1/Game/getAll');
    expect(req.request.method).toBe("GET");
    req.flush(null, { status: 404, statusText: 'Not Found' });
  });

  it('deve chamar getGames() e retornar erro 500', () => {
    service.getGames().subscribe(games => {
      expect(games.length).toBe(0);
    }, error => {
      expect(error.status).toBe(500);
    });

    const req = httpMock.expectOne('https://localhost:7057/api/v1/Game/getAll');
    expect(req.request.method).toBe("GET");
    req.flush(null, { status: 500, statusText: 'Internal Server Error' });
  });

  it('deve chamar getResult() e retornar erro 404', () => {
    const listGames: Game[] = [
      new Game("1", "game1", 1, 1, "url1"),
      new Game("2", "game2", 2, 2, "url2")
    ];

    service.getResult(listGames).subscribe(games => {
      expect(games.length).toBe(0);
    }, error => {
      expect(error.status).toBe(404);
    });

    const req = httpMock.expectOne('https://localhost:7057/api/v1/Game/result');
    expect(req.request.method).toBe("POST");
    req.flush(null, { status: 404, statusText: 'Not Found' });
  });

  it('deve chamar getResult() e retornar erro 500', () => {
    const listGames: Game[] = [
      new Game("1", "game1", 1, 1, "url1"),
      new Game("2", "game2", 2, 2, "url2")
    ];

    service.getResult(listGames).subscribe(games => {
      expect(games.length).toBe(0);
    }, error => {
      expect(error.status).toBe(500);
    });

    const req = httpMock.expectOne('https://localhost:7057/api/v1/Game/result');
    expect(req.request.method).toBe("POST");
    req.flush(null, { status: 500, statusText: 'Internal Server Error' });
  });

  it('deve chamar getGames() e retornar erro 400', () => {
    service.getGames().subscribe(games => {
      expect(games.length).toBe(0);
    }, error => {
      expect(error.status).toBe(400);
    });

    const req = httpMock.expectOne('https://localhost:7057/api/v1/Game/getAll');
    expect(req.request.method).toBe("GET");
    req.flush(null, { status: 400, statusText: 'Bad Request' });
  });
});
