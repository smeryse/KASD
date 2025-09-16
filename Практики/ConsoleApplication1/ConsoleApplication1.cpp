// ConsoleApplication1.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <string>

int main()
{
	const char alphas [] = R"(0123456789ABCDEF)";

	int c = 0;

	for (char a : alphas) {
		for (char b : alphas) {
			for (char c : alphas) {
				for (char d : alphas) {
					for (char e : alphas) {
						for (char f : alphas) {
							for (char g : alphas) {
								for (char h : alphas) {
									if (a == '2' && b == '5' && c == '7')
								}
							}
						}
					}
				}
			}
		}
	}
}
