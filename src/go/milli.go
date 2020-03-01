package main

import (
	"strings"
)

var names []string = []string{
	"sıfır ", "bir", "iki", "üç", "dörd", "beş", "altı", "yeddi", "səkkiz", "doqquz",
	"on", "iyirmi", "otuz", "qırx", "əlli", "altmış", "yetmiş", "səksən", "doxsan", "yüz",
	"min", "milyon", "milyard", "trilyon", "kvadrilyon", "kvintilyon", "sextilyon", "septilyon", "oktilyon",
}

func numToWords(number int, rank int, bankMode bool) string {

	if number == 0 && rank == 0 {
		return names[0]
	}
	if number == 0 {
		return ""
	}
	tail := ""
	digit := number % 10
	if digit != 0 && (bankMode || !(digit == 1 && (rank%3 == 2 || rank == 3 && number < 10))) {
		tail = names[9*(rank%3%2)+digit] + " "
	}
	if rank%3 == 2 && digit > 0 {
		tail += names[19] + " "
	}
	if rank > 0 && rank%3 == 0 && number%1_000 > 0 {
		tail += names[19+rank/3] + " "
	}
	return numToWords(number/10, rank+1, bankMode) + tail
}

// NumToWords - TODO doc
func NumToWords(number int) string {
	return strings.TrimRight(numToWords(number, 0, false), " ")
}

func main() {
	nums := []int{1, 222, 404, 1 << 32}

	for _, num := range nums {
		println(NumToWords(num))
	}
}
