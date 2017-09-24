#!/usr/bin/env bash

printf 'N,C#_min,C#_avg,Native_min,Native_avg\n'
for i in {8..24}
do
	n=1
	for ((j=1; j<=$i; j++))
	do
		n=$(($n * 2))
	done
	printf "$n,"
	t1=$(cd dotnet; dotnet run -c Release -- "$n" | tail -1 | sed s'- ns--g')
	printf "$t1,"
	t2=$(cd native; ./native "$n" | tail -1 | sed s'- ns--')
	printf "$t2\n"
done
