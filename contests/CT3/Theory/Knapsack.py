from typing import List

class KnapsackRecursive:
    """Чисто рекурсивная реализация без мемоизации"""
    def __init__(self, W: int, weights: List[int], values: List[int]):
        self.W = W
        self.n = len(weights)
        self.weights = weights
        self.values = values

    def Solve(self) -> int:
        return self.Recursive(self.n, self.W)

    def Recursive(self, i: int, w: int) -> int:
        if i == 0 or w == 0:
            return 0

        not_take = self.Recursive(i - 1, w)
        take = 0
        if self.weights[i - 1] <= w:
            take = self.values[i - 1] + self.Recursive(i - 1, w - self.weights[i - 1])

        return max(not_take, take)


class KnapsackMyRealization:
    """Рекурсивная реализация с мемоизацией"""
    def __init__(self, W: int, weights: List[int], values: List[int]):
        self.W = W
        self.n = len(weights)
        self.weights = weights
        self.values = values
        self.dp = [[0] * (W + 1) for _ in range(self.n + 1)]

    def Solve(self) -> int:
        return self.Recursive(self.n, self.W)

    def Recursive(self, i: int, w: int) -> int:
        if i == 0 or w == 0:
            return 0

        if self.dp[i][w] != 0:
            return self.dp[i][w]

        not_take = self.Recursive(i - 1, w)
        take = 0
        if self.weights[i - 1] <= w:
            take = self.values[i - 1] + self.Recursive(i - 1, w - self.weights[i - 1])

        self.dp[i][w] = max(not_take, take)
        return self.dp[i][w]


class Knapsack2D:
    """Bottom-up 2D DP"""
    def __init__(self, W: int, weights: List[int], values: List[int]):
        self.W = W
        self.n = len(weights)
        self.weights = weights
        self.values = values
        self.dp = []

    def Solve(self) -> int:
        self.dp = [[0] * (self.W + 1) for _ in range(self.n + 1)]

        for i in range(1, self.n + 1):
            for w in range(1, self.W + 1):
                if self.weights[i - 1] <= w:
                    self.dp[i][w] = max(
                        self.dp[i - 1][w],
                        self.dp[i - 1][w - self.weights[i - 1]] + self.values[i - 1]
                    )
                else:
                    self.dp[i][w] = self.dp[i - 1][w]

        return self.dp[self.n][self.W]

    def GetItems(self) -> List[int]:
        """Восстановление выбранных предметов"""
        result = self.Solve()
        selected_index = []

        w = self.W
        for i in range(self.n, 0, -1):
            if result <= 0:
                break
            if result != self.dp[i - 1][w]:
                selected_index.append(i - 1)
                result -= self.values[i - 1]
                w -= self.weights[i - 1]

        return selected_index


class Knapsack1D:
    # TODO: Разобраться, как работает движение w сверху вниз
    """Bottom-up 1D DP"""
    def __init__(self, W: int, weights: List[int], values: List[int]):
        self.W = W
        self.n = len(weights)
        self.weights = weights
        self.values = values
        self.dp = [0] * (W + 1)

    def Solve(self) -> int:
        for i in range(self.n):
            for w in range(self.W, self.weights[i] - 1, -1):
                self.dp[w] = max(
                    self.dp[w],
                    self.dp[w - self.weights[i]] + self.values[i]
                )
        return self.dp[self.W]

class UnboundedKnapsack1D:
    # TODO: аналогично разобраться почему при движении снизу вверх нескольо элементов мб включены
    """Unbounded Knapsack — каждый предмет можно брать неограниченно"""
    def __init__(self, W: int, weights: List[int], values: List[int]):
        self.W = W
        self.n = len(weights)
        self.weights = weights
        self.values = values
        self.dp = [0] * (W + 1)  # 1D массив для DP

    def Solve(self) -> int:
        # Идём по всем вместимостям от 1 до W
        for w in range(1, self.W + 1):
            for i in range(self.n):
                if self.weights[i] <= w:
                    self.dp[w] = max(
                        self.dp[w],  # не берём текущий предмет
                        self.dp[w - self.weights[i]] + self.values[i]  # берём текущий предмет
                    )
        return self.dp[self.W]



class PSS:
    def __init__(self, target: int, arr: List[int]):
        self.target = target
        self.arr = arr
        self.n = len(arr)
        self.dp = [0] * (target + 1)  # dp[s] = 1, если сумму s можно набрать
        self.dp[0] = 1

    def Solve(self) -> bool:
        for num in self.arr:
            self._update_dp(num)
        return bool(self.dp[self.target])

    def _update_dp(self, num: int):
        """Обновляет dp для одного числа"""
        for s in range(self.target, num - 1, -1):
            if self.dp[s - num]:
                self.dp[s] = 1

    def DebugStep(self):
        """Пошаговая визуализация DP с 1 и 0"""
        print(f"Начальное состояние dp: {self.dp}")
        for num in self.arr:
            self._update_dp(num)
            print(f"После числа {num}: {self.dp}")


        
def Knapsack_example():
    weights = [10, 20, 30]
    values = [60, 100, 120]
    W = 50

    # Тест 2D DP с восстановлением
    knapsack_2d = Knapsack2D(W, weights, values)
    max_value = knapsack_2d.Solve()
    indexes = knapsack_2d.GetItems()
    print("Максимальная стоимость (2D DP):", max_value)  # 220
    print("Выбранные элементы (индексы):", indexes)



def PSS_example():
    arr = [3, 34, 4, 12, 5, 2] 
    target = 9
    pss = PSS(target, arr)
    
    print(f"Визуализация DP для подмножества с суммой {target}:")
    pss.DebugStep()
    
    print(f"\nСуществует ли подмножество с суммой {target}? {pss.Solve()}")

from typing import List, Optional

class Grasshopper:
    def __init__(self, n: int, k: int, coins_input: List[int]):
        """
        :param n: количество столбиков
        :param k: максимальная длина прыжка
        :param coins_input: монеты на столбиках 2..n-1
        """
        self.n = n
        self.k = k
        # coins[1..n], на первом и последнем столбике 0 монет
        self.coins = [0] + coins_input + [0]
        self.dp: List[Optional[int]] = [None] * (n + 1)
        self.prev: List[Optional[int]] = [None] * (n + 1)

    def Solve(self):
        self.dp[1] = 0  # стартовое состояние

        for i in range(2, self.n + 1):
            best = None
            best_prev = None
            for j in range(max(1, i - self.k), i):
                if self.dp[j] is not None:
                    value = self.dp[j] + self.coins[i - 1]
                    if best is None or value > best:
                        best = value
                        best_prev = j
            self.dp[i] = best
            self.prev[i] = best_prev

        # Восстанавливаем путь
        path = []
        cur = self.n
        while cur is not None:
            path.append(cur)
            cur = self.prev[cur]
        path.reverse()

        return self.dp[self.n], path

# ---------------------------
if __name__ == "__main__":
    n = 10
    k = 3
    coins_input = [-13, -2, -14, -124, -9, -6, -5, -7] 
    grasshopper = Grasshopper(n, k, coins_input)
    max_coins, path = grasshopper.Solve()

    print(max_coins)
    print(len(path))
    print(*path)


