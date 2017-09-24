#include <stdlib.h>
#include <stdio.h>
#include <time.h>
#include "func.h"

#define NS_IN_SEC 1000000000LL

struct timespec timer_start(){
    struct timespec start_time;
    clock_gettime(CLOCK_MONOTONIC, &start_time);
    return start_time;
}

int64_t timer_end(struct timespec start){
	struct timespec end;
	clock_gettime(CLOCK_MONOTONIC, &end);

	return ((end.tv_sec - start.tv_sec) * NS_IN_SEC) + (end.tv_nsec - start.tv_nsec);
}

int32_t* randArray(int len)
{
	int32_t *array = malloc(sizeof(int32_t) * len);
	for (int i = 0; i < len; ++i)
	{
		array[i] = rand();
	}

	return array;
}

int main(int argc, char **argv)
{
	if (argc < 2)
	{
		printf("Usage: %s <number>\n", argv[0]);
		exit(1);
	}

#if UINTPTR_MAX == 0xffffffff
	printf("32 bit mode\n");
#elif UINTPTR_MAX == 0xffffffffffffffff
	printf("64 bit mode\n");
#else
/* wtf */
#endif

	int32_t len = atoi(argv[1]);

	int64_t min = 0x7FFFFFFFFFFFFFFF;
	int64_t total = 0;

	for (int test = 0; test < 100; ++test)
	{
		int32_t *x = randArray(len);
		int32_t *y = randArray(len);

		volatile int32_t *res = malloc(sizeof(int32_t) * len);

		struct timespec start = timer_start();
		multi_arr(x, y, (int32_t*)res, len);
		int64_t t = timer_end(start);
		total += t;

		if (t < min)
		{
			min = t;
		}

		// clean up
		free(x); x = NULL;
		free(y); y = NULL;
		free((int32_t*)res); res = NULL;
	}

	printf("%ld ns,", min);
	printf("%ld ns\n", total / 100);
}
