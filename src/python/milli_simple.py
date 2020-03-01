from __future__ import division

_names = ('sıfır', 'bir', 'iki', 'üç', 'dörd', 'beş', 'altı', 'yeddi', 'səkkiz', 'doqquz',
          'on', 'iyirmi', 'otuz', 'qırx', 'əlli', 'altmış', 'yetmiş', 'səksən', 'doxsan', 'yüz',
          'min', 'milyon', 'milyard', 'trilyon', 'kvadrilyon', 'kvintilyon', 'sextilyon', 'septilyon', 'oktilyon',
          )

def num_to_words(num, rank=0, bank_mode=False):
    if num == 0 and rank == 0:
        return _names[0]
    if num == 0:
        return ''
    digit = num % 10
    tail = ''
    if digit != 0 and (bank_mode or not (digit == 1 and (rank % 3 == 2 or rank == 3 and num < 10))):
        tail = _names[9 * (rank % 3 % 2) + digit] + ' '
    if rank % 3 == 2 and digit > 0:
        tail += _names[19] + ' '
    if rank > 0 and rank % 3 == 0 and num % 1000 > 0:
        tail += _names[19 + rank // 3] + ' '
    return num_to_words(num // 10, rank + 1, bank_mode) + tail


if __name__ == '__main__':
    test_nums = [0, 7, 18, 99, 100, 165, 404, 999, 1000, 1001, 2020, 1 << 32]

    for num in test_nums:
        words = num_to_words(num)
        print(num, '->', words)

    print('=' * 50)
    print('natural mode vs banking mode:')
    print(125, '(natural): ', num_to_words(125))
    print(125, '(banking): ', num_to_words(125, bank_mode=True))
