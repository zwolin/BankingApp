Zadanie testowe
Cel zadania: 

Utworzenie aplikacji ASP.NET Core Web API (preferowana wersja 6.0) z
wykorzystaniem Entity Framework (w modelu Code First). Serwis ma symulować zachowanie
systemu transakcyjnego (bankowość), ograniczając się do zagadnień związanych z operacjami
na saldzie bankowym z uwzględnieniem zabezpieczeń przed nieprawidłowymi operacjami
(np. jednoczesne operacje na tym samym saldzie):
* wypłata pieniędzy
* wpłata pieniędzy
* stan konta (saldo)
Dodatkowym autem będzie:
* utworzenie testów jednostkowych
* zastosowanie dowolnego sposobu autoryzacji użytkownika (np. JWT)
* zastosowanie wzorca Mediator / architektury CQRS i Event Sourcing
* dostarczenie rozwiązania w środowisku dockerowym
* asynchroniczne powiadomienia o wykonaniu operacji (np. SignalR)


Zadanie testowe proszę umieścić na dowolnym serwisie hostującym repozytoria GIT (Github /
Gitlab / Bitbucket / Azure Repos) lub wysłać spakowaną solucję z kodem źródłowym.
