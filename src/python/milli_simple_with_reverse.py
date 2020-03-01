from __future__ import division

_names = ('sıfır', 'bir', 'iki', 'üç', 'dörd', 'beş', 'altı', 'yeddi', 'səkkiz', 'doqquz',
          'on', 'iyirmi', 'otuz', 'qırx', 'əlli', 'altmış', 'yetmiş', 'səksən', 'doxsan', 'yüz',
          'min', 'milyon', 'milyard', 'trilyon', 'kvadrilyon', 'kvintilyon', 'sextilyon', 'septilyon', 'oktilyon',
          )

values = dict(
    [(name, (i % 10) * (10 ** (i // 10)) + ((i // 10) * 10)) for i, name in enumerate(_names[:10+10])] +
    [(name, 1000 ** (i + 1)) for i, name in enumerate(_names[10+10:])]
)
# or you can define manually if you ain't lazy to write: values = {'sıfır': 0, 'bir': 1, ...}


# 512 -> beş yüz on iki
def num_to_words(num, rank=0, bank_mode=False):
    if num == 0:
        if rank == 0:
            return _names[0]
        return ''
    digit = num % 10
    result = ''
    if digit != 0 and (bank_mode or not (digit == 1 and (rank % 3 == 2 or rank == 3 and num < 10))):
        result = _names[9 * (rank % 3 % 2) + digit] + ' '
    if rank % 3 == 2 and digit > 0:
        result += _names[19] + ' '
    if rank > 0 and rank % 3 == 0 and num % 1000 > 0:
        result += _names[19 + rank // 3] + ' '
    return num_to_words(num // 10, rank + 1, bank_mode) + result


# beş yüz on iki -> 512
def words_to_num(s):
    num = 0
    carry = 0
    for node in s.split():
        v = values[node]
        if v < 100:
            carry += v
        elif v == 100:
            carry = v * max(1, carry)
        else:
            num += max(1, carry) * v
            carry = 0
    return num + carry


if __name__ == '__main__':

    for x in range(1, 1000001):
        words = num_to_words(x)
        revert = words_to_num(words)
        if x != revert:
            print('found a bug:', x)
            break
        print('OK:', x, '->', words, '->', revert)
    else:
        print('works like a charm!')

    print('extra:')
    print(words_to_num('iki min qırx səkkiz'))
