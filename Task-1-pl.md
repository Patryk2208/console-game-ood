# Etap 1: Podstawowy program gry

## Cel zadania

Stworzenie konsolowej wersji gry RPG, w której:
1. Gracz porusza się po jednym pomieszczeniu.
2. Gracz zbiera przedmioty, które mogą być modyfikowane za pomocą modyfikatorów (**Decorator**).

## Wymagania funkcjonalne

1. **Pomieszczenie:**
   - Plansza gry to prostokąt o stałym rozmiarze  **20x40** pól.
   - Każda komórka planszy może być:
     - pusta (` `),
     - ścianą (`█`),
     - zawierać gracza (`¶`),
     - zawierać wiele przedmiotów (dowolny symbol, zależny od przedmiotów).
   - Pozycja początkowa gracza: **(0, 0)**.
   - Na tym etapie nie jest wymagana generacja labiryntu ani rozmieszczenia przedmiotów.
     Przedmioty i ściany mogą być ułożone w predefiniowany sposób.

2. **Gracz:**
   - Porusza się w czterech kierunkach (kontrola literami `W`, `S`, `A`, `D`).
   - Nie może wyjść poza granice planszy.
   - Nie może przechodzić przez ściany.
   - Ma różne atrybuty, takie jak siła (P), zręczność (A), zdrowie (H), szczęście (L), agresja (A) i mądrość (W).
   - Ma dwie ręce, w każdej może umieścić przedmiot.

3. **Przedmioty:**
   - W pomieszczeniu rozmieszczone są zdefiniowane w kodzie przedmioty.
   - Są przynajmniej trzy rodzaje broni, w tym jedna dwuręczna. Broń ma wartość obrażeń.
   - Są przynajmniej trzy rodzaje nieużywalnych przedmiotów.
   - Są dwa rodzaje waluty: monety i złoto.
   - Każda broń ma obrażenia, które zadaje przy użyciu (na tym etapie jeszcze nie implementujemy użycia broni, tylko zakładanie).
   - Jeśli gracz stanie na polu z przedmiotem, to może go podnieść naciskając przycisk `E`.

4. **Efekty:**
   - Przedmioty mogą być modyfikowane dodatkowymi efektami.
     - Przykładowo:
       - **"Pechowy"**: -5 do szczęścia noszącego.
       - **"Silny"**: +5 do obrażeń broni. Każdy przedmiot może mieć kilka efektów.
   - Wszystkie efekty uwzględnione powinny być w nazwie przedmiotu (np. ,,Miecz (Pechowy) (Ochronny)").
   - Powinien być zdefiniowany przynajmniej jeden efekt wpływający na obrażenia broni i przynajmniej jeden wpływający na atrybuty gracza.
   - Efekty są nakładane na przedmioty w momencie ich tworzenia,
        t.j. w przyszłości w momencie generowania świata w sposób losowy,
        ale na tym etapie może to być jeszcze ustalone w kodzie.
   - Do implementacji tej funkcjonalności należy użyć wzorca **dekorator** (to **bardzo** ważne i jest istotą zadania -- bez użycia wzorca odbieramy 80% punktów).

5. **Wyświetlanie stanu gry:**
   - Plansza jest rysowana w konsoli.
   - Obok planszy wyświetlane są:
     - ekwipunek,
     - aktualnie używane przedmioty,
     - jeśli gracz jest na polu, na którym jest przedmiot, to informacja o tym,
     - aktualne wartości atrybutów gracza,
     - liczba zebranych monet i złota.

6. **Ekwipunek**
   - Gracz może zarządzać swoim ekwipunkiem, t.j.:
     - wyrzucać przedmioty na ziemię,
     - wyciągać i wkładać przedmioty do obu rąk (poprawna obsługa broni dwuręcznych!).

