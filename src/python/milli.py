from __future__ import division
from decimal import Decimal
import re

# order matters
_names = ('sıfır', 'bir', 'iki', 'üç', 'dörd', 'beş', 'altı', 'yeddi', 'səkkiz', 'doqquz',
          'on', 'iyirmi', 'otuz', 'qırx', 'əlli', 'altmış', 'yetmiş', 'səksən', 'doxsan',
          'yüz',
          'min', 'milyon', 'milyard', 'trilyon', 'kvadrilyon', 'kvintilyon', 'sextilyon', 'septilyon', 'oktilyon',
          # extendable
    )

values = dict(
    [(name, (i % 10) * (10 ** (i // 10)) + ((i // 10) * 10)) for i, name in enumerate(_names[:10 + 10])] +
    [(name, 1000 ** (i + 1)) for i, name in enumerate(_names[10 + 10:])]
)

_max_rank = (len(_names) - 19) * 3
_max_dec_places = 14  # that python can handle properly
_vowels = 'aıoueəiöü'
v4map = {'a': 'ı', 'o': 'u', 'e': 'i', 'ə': 'i', 'ö': 'ü'}

def _adop_suffix(root: str, suffix: str):
    i = max(root.rfind(v) for v in _vowels)
    if i == -1:
        return suffix
    last_vowel = root[i]
    is_high = _vowels.index(last_vowel) > 3 # incə sait
    v2 = 'ə' if is_high else 'a'
    v4 = v4map.get(last_vowel, last_vowel)
    suffix = re.sub('[aə]', v2, suffix)
    suffix = re.sub('[ıiuü]', v4, suffix)
    if is_high and suffix[-1] == 'q':
        suffix = suffix[:-1] + 'k'
    return suffix

# kök və şəkilçini ahəng qanununa uyğun birləşdirir
def _add_suffix(root: str, suffix: str):
    suffix = _adop_suffix(root, suffix)
    # şəkilçi saitlə başlayırsa, kökdə sonuncu sait düşür
    root = root[:-1] if root[-1] in _vowels and suffix[0] in _vowels else root
    return root + suffix

def _num_to_words(num, bank_mode=False):
    if num == 0:
        return _names[0]
    s = ''
    i = 0
    while num and i < _max_rank:
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
    s = s[:-1] # trim space
    if num != 0:  # in the case of overflow keep remaining numbers as is
        s = '{} {}'.format(num, s)
    return s

def num_to_words(num, bank_mode=False, positive_sign=False, decimal_places=None):
    int_part = int(num)
    sign = ''
    if num < 0:
        sign = 'mənfi '
    elif num and positive_sign:
        sign = 'müsbət '
    int_name = sign + _num_to_words(abs(int_part), bank_mode)

    rounded = str(round(abs(num), max(0, min(decimal_places or _max_dec_places, _max_dec_places))))
    _, digits, exp = Decimal(rounded).normalize().as_tuple()
    if exp >= 0:
        return int_name
    nom = int(''.join(map(str, digits[exp:])))
    denom = 10 ** -exp
    nom_name = _num_to_words(nom)
    denom_name = _add_suffix(_num_to_words(denom), 'da')
    result = '{} tam {} {}'.format(int_name, denom_name, nom_name)
    return result

def _words_to_num(s):
    num = 0
    carr = 0
    for node in s.split():
        v = values[node]
        if v < 100:
            carr += v
        elif v == 100:
            carr = v * max(1, carr)
        else:
            num += max(1, carr) * v
            carr = 0
    return num + carr

def words_to_num(words):
    sign = 1
    if words.startswith(('mənfi ', 'müsbət ')):
        sign, words = words.split(' ', maxsplit=1)
        sign = -1 if sign == 'mənfi' else 1
    if ' tam ' not in words:
        return _words_to_num(words) * sign
    int_part, dec_part = words.split(' tam ')
    dec_words = dec_part.split(' ')
    idx = next(i for i, w in enumerate(dec_words, start=1) if w.endswith(('da', 'də')))
    denom, nom = dec_words[:idx], dec_words[idx:]
    denom.append(denom.pop()[:-2])  # remove suffix
    int_v = _words_to_num(int_part) * sign
    denom_v = _words_to_num(' '.join(denom)) * sign
    nom_v = _words_to_num(' '.join(nom))
    return round(int_v + nom_v / denom_v, _max_dec_places)


def ordinal(num, full=False):
    root = num_to_words(num)
    if full:
        return _add_suffix(root, 'ıncı')
    return '{}-{}'.format(num, _adop_suffix(root, 'cı'))


def num_to_currency(num, bank_mode=False, nominal='manat', coin_unit='qəpik'):
    int_part = int(num)
    int_name = _num_to_words(abs(int_part), bank_mode)
    coin_part = str(int(num * 100)/100.0).split('.')[-1].ljust(2, '0')
    coins = int(coin_part)
    coins_name = _num_to_words(coins, bank_mode)
    if not bank_mode and not coins:
        return '{} {}'.format(int_name, nominal)
    if not bank_mode and not int_part:
        return '{} {}'.format(coins_name, coin_unit)
    return '{} {} {} {}'.format(int_name, nominal, coins_name, coin_unit)

if __name__ == '__main__':

    print('=' * 30, 'number to words', '=' * 30)
    numbers = [0, 1, 2, 3, 3.14, 86.6, 99.99, -273.15, 1<<32]

    for number in numbers:
        print(number, '->', num_to_words(number))


    print('=' * 30, 'ordinals', '=' * 30)
    ordinals = [1, 2, 3, 4, 5, 6, 10, 50, 100, 1995]

    for number in ordinals:
        print(number, '->', ordinal(number))


    print('=' * 30, 'ordinals (full)', '=' * 30)
    for number in ordinals:
        print(number, '->', ordinal(number, full=True))
    

    print('=' * 30, 'num_to_currency', '=' * 30)
    pulla_bua_ve = [50, 12.5, 0.2, 99.99]
    for m in pulla_bua_ve:
        print(m, '->', num_to_currency(m))    
    
    
    print('=' * 30, 'words to number', '=' * 30)
    words_list = [
        'on', 
        'yetmiş yeddi', 
        'səksən altı tam onda altı', 
        'mənfi iki yüz iyirmi iki tam on mində yüz iyirmi beş'
    ]

    for words in words_list:
        number = words_to_num(words)
        print(words, '->', number)
