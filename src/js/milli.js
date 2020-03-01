const names = ['sıfır', 'bir', 'iki', 'üç', 'dörd', 'beş', 'altı', 'yeddi', 'səkkiz', 'doqquz',
          'on', 'iyirmi', 'otuz', 'qırx', 'əlli', 'altmış', 'yetmiş', 'səksən', 'doxsan', 'yüz',
          'min', 'milyon', 'milyard', 'trilyon', 'kvadrilyon', 'kvintilyon', // tr.wikipedia.org/wiki/Büyük_sayıların_adları
          ];
function _numToWords(number, rank, bankMode) {
    if (number == 0 && rank == 0)
        return names[0];
    if (number == 0)
        return '';
    let tail = '';
    let digit = number % 10;
    if (digit != 0 && (bankMode || !(digit == 1 && (rank % 3 == 2 || rank == 3 && number < 10))))
        tail = names[9 * (rank % 3 % 2) + digit] + ' ';
    if (rank % 3 == 2 && digit > 0)
        tail += names[19] + ' ';
    if (rank > 0 && rank % 3 == 0 && number % 1_000 > 0)
        tail += names[19 + rank / 3] + ' ';
    return _numToWords(Math.trunc(number / 10), rank + 1, bankMode) + tail;
}
// 135 -> yüz otuz beş, 1001 -> min bir
numToWords = (number) => _numToWords(number, 0, false).trim();
// 135 -> bir yüz otuz beş, 1001 -> bir min bir
numToWordsBankMode = (number)  => _numToWords(number, 0, true).trim();

// test
console.log(numToWords(35));
console.log(numToWords(444));
console.log(numToWords(2665123));
