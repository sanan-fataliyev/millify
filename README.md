## Millify - ədədi yazılı formaya çevirən alqoritmlər and more

#### natural ədədləri yazılı formaya çevirən minimal alqoritm (python):
```python
names = ('sıfır', 'bir', 'iki', 'üç', 'dörd', 'beş', 'altı', 'yeddi', 'səkkiz', 'doqquz',
          'on', 'iyirmi', 'otuz', 'qırx', 'əlli', 'altmış', 'yetmiş', 'səksən', 'doxsan', 'yüz',
          'min', 'milyon', 'milyard', 'trilyon', 'kvadrilyon', 'kvintilyon', )

def num_to_words(num, bank_mode=False, rank=0):
    if num == 0 and rank == 0:
        return names[0]
    if num == 0:
        return ''
    digit = num % 10
    tail = ''
    if digit != 0 and (bank_mode or not (digit == 1 and (rank % 3 == 2 or rank == 3 and num < 10))):
        tail = names[9 * (rank % 3 % 2) + digit] + ' '
    if rank % 3 == 2 and digit > 0:
        tail += names[19] + ' '
    if rank > 0 and rank % 3 == 0 and num % 1000 > 0:
        tail += names[19 + rank // 3] + ' '
    return num_to_words(num // 10, bank_mode, rank + 1) + tail
```

#### istifadə:
```python
print(num_to_words(35566)) # otuz beş min beş yüz altmış altı 
print(num_to_words(987654321)) # doqquz yüz səksən yeddi milyon altı yüz əlli dörd min üç yüz iyirmi bir 

```

---


## Toplam imkanlar:

```
============================== number to words ==============================
0 -> sıfır
1 -> bir
2 -> iki
3 -> üç
3.14 -> üç tam yüzdə on dörd
86.6 -> səksən altı tam onda altı
99.99 -> doxsan doqquz tam yüzdə doxsan doqquz
-273.15 -> mənfi iki yüz yetmiş üç tam yüzdə on beş
4294967296 -> dörd milyard iki yüz doxsan dörd milyon doqquz yüz altmış yeddi min iki yüz doxsan altı
============================== ordinals ==============================
1 -> 1-ci
2 -> 2-ci
3 -> 3-cü
4 -> 4-cü
5 -> 5-ci
6 -> 6-cı
10 -> 10-cu
50 -> 50-ci
100 -> 100-cü
1995 -> 1995-ci
============================== ordinals (full) ==============================
1 -> birinci
2 -> ikinci
3 -> üçüncü
4 -> dördüncü
5 -> beşinci
6 -> altıncı
10 -> onuncu
50 -> əllinci
100 -> yüzüncü
1995 -> min doqquz yüz doxsan beşinci
============================== num_to_currency ==============================
50 -> əlli manat
12.5 -> on iki manat əlli qəpik
0.2 -> iyirmi qəpik
99.99 -> doxsan doqquz manat doxsan doqquz qəpik
============================== words to number ==============================
on -> 10
yetmiş yeddi -> 77
səksən altı tam onda altı -> 86.6
mənfi iki yüz iyirmi iki tam on mində yüz iyirmi beş -> -222.0125
```

vəə biraz fan 
<img src="img/plain_math.gif">
---
Maksimum əlçatanlıq üçün aşağıdaki dillərdə kodlar əlavə etdim:
- [python (tam)](/src/python/milli.py)
- [c# (yarımçıq)](/src/csharp/Millify/Milli.cs)
- [javascript (yarımçıq)](src/js/milli.js)
- [Go (yarımçıq)](/src/go/milli.go)


### Təbii vs Bank mode
Alqoritm həm təbii, həm də "bank" modunu dəstəkləyir.
 Təbii danışıqda `100` və `1000` uyğun olaraq `"yüz"` və `"min"` kimi işlənsə də, bir çox rəsmi sənədlərdə, həmçinin bank çıxarışlarında `bir yüz`, `bir min` kimi yazılır. Bu, birmənalılığı saxlamağa xidmət edir. Ya da ki, proqramçılar üçün bucür yazmaq daha straightforward olub :)
#### Nümunə üçün:

Ədəd|Təbii|Bank mode
:---|:---:|:---:
100  | yüz                 | **bir** yüz 
125  | yüz iyirmi beş      | **bir** yüz iyirmi beş
250  | iki yüz əlli        | iki yüz əlli 
1024 | min iyirmi dörd     | **bir** min iyirmi dörd
51000|əlli bir min         | əlli bir min
1000000| bir milyon        | bir milyon


## Mümkün tətbiqləri
### Mətn səsləndirilməsində (text-to-speech)
Səsləndirilmədən əvvəl mətnlər normalizasiya olunur. Bu zaman [ədədlər yazılı formaya keçməli olur.](https://en.wikipedia.org/wiki/Speech_synthesis#Text_normalization_challenges)
Bu məqsədlə alqoritm yerli NLP sistemlərimizin text-to-speech moduluna tətbiq oluna bilər.

Əfsanəvi dilmanc sistemi (http://dilmanc.az/az/metnin-seslendirmesi) yalnız 1 milyondan aşağı ədədləri səsləndirə bilir. Onluq kəsr hissənin səsləndirilməsi *yoxdur*. Təbii səsləndirmə stabil işləmir (məs: `120` - `yüz iyirmi`, amma `120000` - `bir yüz iyirmi min` kimi səslənir)

Bununla yanaşı, əks alqoritm - `words_to_num` speech-to-text modulunda istifadə ola bilər.

### Elektron sənəd generasiyasında
Elektron sənədlərdə məbləğ və digər miqdarları yazılı formaya programmatically çevirmək.

PS. Excel funksiya kimi yazmaq faydalı olardı. Mən excel üçün macro yazmağa çalışdım, amma Visual Basic sintaksisi beyin xərçənginə yol açırdı deyə davam edə bilmədim.

### Səsli növbə sisteminin qurulmasında

### Tested?
Əks alqorimtlər (`num_to_words` və `words_to_num` ) bir birini sıfırdan bir milyona qədər tam ədədlərlə test edib. Həmçinin, edge case-ləri yoxlamaq üçün [100+ test var (c#, xUnit)](/src/csharp/Millify.Tests/TestSpell.cs)

### Limitasiyalar
Kəsr hissəyə aid alqoritmlər experimentaldır. Onluq kəsrləri floating point sistemə xətasız keçirmək olmur. Kəsr ədədi string kimi qəbul edərək bu məsələni həll etmək olar.

Min və sonrakı bütün minlik mərtəbələrdə əmsallaşdırma məntiqini eyni olduğu üçün alqoritm çox böyük ədədlərlə də işləyəcək. Nəzəri olaraq istənilən qədər [minlik mərtəbə](https://tr.wikipedia.org/wiki/Büyük_sayıların_adları) əlavə etmək olar. Mərtəbənin adını adlar siyahısına əlavə etmək kifayətdir. 
