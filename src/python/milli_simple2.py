from __future__ import division

_names = ('sıfır', 'bir', 'iki', 'üç', 'dörd', 'beş', 'altı', 'yeddi', 'səkkiz', 'doqquz',
          'on', 'iyirmi', 'otuz', 'qırx', 'əlli', 'altmış', 'yetmiş', 'səksən', 'doxsan', 'yüz',
          'min', 'milyon', 'milyard', 'trilyon', 'kvadrilyon', 'kvintilyon', 'sextilyon', 'septilyon', 'oktilyon',
          )


def num_to_words(num, bank_mode=False):
    if num == 0:
        return _names[0]
    s = ''
    i = 0
    while num:
        local = ''
        digit = num % 10
        if digit != 0 and (bank_mode or not (digit == 1 and (i % 3 == 2 or i == 3 and num < 10))):
            local = _names[9 * (i % 3 % 2) + digit] + ' '
        if i % 3 == 2 and digit > 0:
            local += _names[19] + ' '
        if i > 0 and i % 3 == 0 and num % 1000 > 0:
            local += _names[19 + i // 3] + ' '
        s = local + s
        num //= 10
        i += 1
    return s[:-1]  # trim space


if __name__ == '__main__':
    test_nums = [0, 7, 18, 99, 100, 165, 404, 999, 1000, 1001, 2020, 1 << 32]

    for num in test_nums:
        words = num_to_words(num)
        print(num, '->', words)

    print('=' * 50)
    print('natural mode vs banking mode:')
    print(125, '(natural): ', num_to_words(125))
    print(125, '(banking): ', num_to_words(125, bank_mode=True))
