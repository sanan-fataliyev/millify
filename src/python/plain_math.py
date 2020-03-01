from milli import words_to_num, num_to_words
import re

ops = {'üstü': '**', 'vur': '*', 'böl': '/', 'üstəgəl': '+', 'çıx': '-', '(': '(', ')': ')'}
ops = {**ops, **{v: v for v in ops.values()}}

tokenizer = '(' + '|'.join(map(re.escape, ops.keys())) + ')'


def solve_plain_math(math_exp):
    tokens = [token.strip() for token in re.split(tokenizer, math_exp)]
    tokens = [token for token in tokens if token]
    nodes = []
    for token in tokens:
        if token in ops:
            nodes.append(ops[token])
        else:
            try:
                nodes.append(repr(words_to_num(token)))
            except:
                return "error '{}'".format(token)

    py_exp = ' '.join(nodes)
    val = eval(py_exp)
    return num_to_words(val)


if __name__ == '__main__':

    while True:
        exp = input('misal: ') # beş üstü üç çıx ( on iki tam yüzdə iyirmi beş vur altı ) üstəgəl on yeddi
        print(solve_plain_math(exp))
