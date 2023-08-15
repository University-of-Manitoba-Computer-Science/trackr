all: libs example

example: src/example.cpp lib/trackr.a
	g++ src/example.cpp -o example -Llib -l:trackr.a -lcurl

trackr.o: lib/trackr.cpp lib/trackr.hpp
	g++ -c lib/trackr.cpp -o lib/trackr.o

trackr.a: trackr.o
	ar rcs lib/trackr.a lib/trackr.o

libs: trackr.a

cleanlib: 
	rm lib/trackr.a lib/trackr.o

cleanexample:
	rm example

clean: cleanlib cleanexample