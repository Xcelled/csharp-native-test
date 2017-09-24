#include "func.h"

void multi_arr(int32_t *x, int32_t *y, int32_t *res, int32_t len)
{
	for (int32_t i = 0; i < len; ++i)
	{
		res[i] = x[i] * y[i];
	}
}
