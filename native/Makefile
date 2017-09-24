CC=gcc
CFLAGS= -Wall -O3 -funroll-loops -march=native
RM= rm -f

.PHONY: all clean

all: libfunc.so native
clean:
	$(RM) *.so native

libfunc.so: func.c
	$(CC) $(CFLAGS) -fPIC -shared $< -o $@

native: driver.c func.c
	$(CC) $(CFLAGS) $^ -o $@
