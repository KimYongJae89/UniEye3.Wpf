#include "HostFunction.h"
#include <algorithm>

using namespace std;

bool hostEdgeDetectX(
	float* pProfile,
	int rangeMin, int rangeMax,
	int maxSize,
	const float threshold, 
	int* pStartPos, int* pEndPos, 
	int searchRange)
{
	bool isFindStart = pStartPos == NULL;
	bool isFindEnd = pEndPos == NULL;

	if (rangeMin < 0)
		rangeMin = 0;

	for (int x = rangeMin; x < rangeMax; x += searchRange)
	{
		if (!isFindStart && pStartPos != NULL)
		{
			if (pProfile[x] > threshold && *pStartPos > x)
			{
				isFindStart = true;

				if (searchRange == 1)
					*pStartPos = x;
				else
				{
					int findMin = x - searchRange;
					int findMax = x;

					int newSearchRange = (findMax - findMin) / 10;
					if (newSearchRange == 0)
						newSearchRange = 1;

					//if (findMin < rangeMin)
					//	findMin = rangeMin;

					if (findMin < rangeMin)
					{
						int diff = rangeMin - findMin;
						findMin += diff;
						findMax += diff;
					}

					hostEdgeDetectX(pProfile, findMin, findMax + 1, maxSize, threshold, pStartPos, NULL, newSearchRange);
				}
			}
		}

		if (!isFindEnd && pEndPos != NULL)
		{
			int revX = maxSize - x - 1;

			if (pProfile[revX] > threshold && *pEndPos < revX)
			{
				isFindEnd = true;

				if (searchRange == 1)
					*pEndPos = revX;
				else
				{
					int findMin = x - searchRange;
					int findMax = x;

					int newSearchRange = (findMax - findMin) / 10;
					if (newSearchRange <= 10)
						newSearchRange = 1;

					if (findMin < rangeMin)
					{
						int diff = rangeMin - findMin;
						findMin += diff;
						findMax += diff;
					}

					hostEdgeDetectX(pProfile, findMin, findMax, maxSize, threshold, NULL, pEndPos, newSearchRange);
				}
			}
		}
	}

	// 위치를 못찾으면 예외값을 리턴시킨다.
	if (!isFindStart && pStartPos != NULL)
	{
		if (searchRange == 1)
			*pStartPos = rangeMax;
		else
			hostEdgeDetectX(pProfile, rangeMin, rangeMax, maxSize, threshold, pStartPos, NULL, 1);
	}

	if (!isFindEnd && pEndPos != NULL)
	{
		if (searchRange == 1)
			*pEndPos = maxSize - rangeMin - 1;
		else
			hostEdgeDetectX(pProfile, rangeMin, rangeMax, maxSize, threshold, NULL, pEndPos, 1);
	}

	return isFindStart && isFindEnd;
}
