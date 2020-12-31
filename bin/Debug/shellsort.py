def shellSort(arr) -> list:
    n = len(arr)
    gap = n // 2
    while gap > 0:
        for i in range(gap, n):
            temp = arr[i]
            j = i
            while j >= gap and arr[j - gap] > temp:
                arr[j] = arr[j - gap]
                j -= gap
            arr[j] = temp
        gap //= 2
    return arr

if __name__ == "__main__":
    a = shellSort([123123,123,1242,124,151,25,463,143,35,241,241,35124])
    print(a)