using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace EX
{
    class Program
    {
        static void Main(string[] args)
        {
            int Password = 1118; //관리자 모드로 들어가기 위한 비밀번호
            int ShutDown = 00700; //자판기 종료를 위함
            VendingMachine VM = new VendingMachine();
            VM.DefaultSetting();

            while (true)
            {
                Console.WriteLine("1118을 입력하시면 관리자 모드로 진입합니다");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("원하시는 기능을 선택해주세요");
                Console.WriteLine("1번 : 제품보기");
                Console.WriteLine("2번 : 제품구매");
                Console.WriteLine("3번 : 현금 투입");
                Console.WriteLine("4번 : 잔액 반환");
                int num = int.Parse(Console.ReadLine());
                if (num == Password)
                {
                    VM.ManagerTurnel();
                }
                else if (num == ShutDown)
                {
                    return;
                }
                else if (num == 1)
                {
                    VM.ShowProduct();
                }
                else if (num == 2)
                {
                    VM.SelectProduct();
                }
                else if (num == 3)
                {
                    Console.WriteLine("현금을 넣어주세요");
                    VM.InsertCash(int.Parse(Console.ReadLine()));
                    Console.WriteLine();
                }
                else if (num == 4)
                {
                    foreach (var coin in VM)
                    { }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요");
                }
            }

        }

    }

    public class Product
    {
        public ProductInfo productInfo = new ProductInfo();

        public void AddProductInfo(string name, int price)
        {
            productInfo.Name = name;
            productInfo.Price = price;
        }
        public struct ProductInfo
        {
            public string Name;
            public int Price;
        }
    }


    public class VendingMachine : IEnumerable
    {
        public static int SpaceLimit = 16; //자판기의 제품공간 제한
        public SellInfo[] Products = new SellInfo[SpaceLimit]; //제품 관리
        public int Cash = 0; //현재 투입된 금액
        private int TotalChanges = 0; //자판기내 현재 잔돈의 총액
        private int[] CoinUnits = new int[5] { 1000, 500, 100, 50, 10 }; //입력가능한 돈의 범위
        public Dictionary<int, int> Changes = new Dictionary<int, int>(); //현재 잔돈들의 갯수

        public struct SellInfo
        {
            public Product product;
            public int Amount;
            public int TotalSellAmount;
        }

        //최초셋팅
        public void DefaultSetting()
        {
            foreach(int coin in CoinUnits)
            {                
                Changes.Add(coin, 0);
            }
        }
        public void InsertCash(int _cash)
        {
            if (IscollectCoins(_cash))
            {
                bool isover = false;
                Cash += _cash;
                Changes[_cash] = Changes[_cash] + 1;
                ShowCashes();
                while (!isover)
                {
                    Console.WriteLine("추가 투입을 원하시면 금액을 입력해주세요");
                    Console.WriteLine("아니라면 0을 입력해주세요");
                    int num = int.Parse(Console.ReadLine());
                    if (num != 0)
                    {
                        if (IscollectCoins(num))
                        {
                            Cash += num;
                            Changes[num] = Changes[num] + 1;
                            ShowCashes();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        //잔돈 반환
        public IEnumerator GetEnumerator()
        {
            int total = Cash;
            if (Cash > TotalChanges)
            {
                Console.WriteLine("자판기 내부 잔돈이 부족합니다 관리자에게 연락해주세요");
                Cash = 0;
                yield break;
            }


            foreach (int Coin in CoinUnits)
            {
                int amount = Changes[Coin];
                if (Cash / Coin <= amount)
                {
                    Console.WriteLine("{0}원권 {1} 매/개", Coin, Cash / Coin); 
                    amount -= (Cash / Coin);
                    Cash -= (Cash / Coin) * Coin;
                    Changes[Coin] = amount;
                }
                else
                {
                    Console.WriteLine("{0}원권 {1} 매/개", Coin, Cash / Coin);
                    amount -= amount;
                    Cash -= Changes[Coin] * amount;
                    Changes[Coin] = amount;
                }
            }
            Console.WriteLine("{0}원 잔돈 배출 완료", total);
            yield break;
        }


        //투입한 돈의 범위가 올바른지 확인, CoinUnits
        public bool IscollectCoins(int coin)
        {
            for (int i = 0; i < CoinUnits.Length; i++)
            {
                if (CoinUnits[i] == coin)
                {
                    return true;
                }
            }
            Console.WriteLine("잘못된 현금입력입니다.");
            Console.WriteLine("천원권, 오백원, 백원, 오십원, 십원만 입력가능");
            return false;

        }

        public void ManagerTurnel()
        {
            Console.WriteLine("1 : 제품 추가");
            Console.WriteLine("2 : 제품 제거");
            Console.WriteLine("3 : 제품 정렬");
            Console.WriteLine("4 : 총 판매량");
            Console.WriteLine("5 : 제품 찾기");
            Console.WriteLine("6 : 잔돈 추가");
            Console.WriteLine("7 : 현재 잔돈 보기");
            Console.WriteLine("8 : 종료");

            bool istrue = false;
            while (!istrue)
            {
                int FuncNumber = int.Parse(Console.ReadLine());
                if (FuncNumber == 1)
                {
                    Add();
                }
                else if (FuncNumber == 2)
                {
                    ShowProduct();
                    Console.WriteLine();
                    Console.WriteLine("제거하고자 하는 상품 번호를 골라주세요");
                    int num = int.Parse(Console.ReadLine()) - 1;
                    Remove(num);
                }
                else if (FuncNumber == 3)
                {
                    Sort(Products);
                }
                else if (FuncNumber == 4)
                {
                    ShowTotalSales();
                }
                else if (FuncNumber == 5)
                {
                    Find();
                }
                else if (FuncNumber == 6)
                {
                    InsertChanges();
                }
                else if (FuncNumber == 7)
                {
                    ShowChanges();
                }
                else if (FuncNumber == 8)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력 해 주세요");
                }

                while (true)
                {
                    Console.WriteLine("관리자 모드 계속 : 1, 관리자 모드 종료 : 2");
                    FuncNumber = int.Parse(Console.ReadLine());
                    if (FuncNumber == 1)
                    {
                        break;
                    }
                    else if (FuncNumber == 2)
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                    }
                }
            }
        }

        /// <summary>
        /// 중복 확인
        /// </summary>
        public void FindOverride(string _productName, int _amount, int _price)
        {
            //중복 확인
            for (int i = 0; i < Products.Length; i++)
            {
                if (Products[i].product != null)
                {
                    if (Products[i].product.productInfo.Name == _productName)
                    {
                        int FuncNum = ProductCheck(_productName, _price);

                        //제품 문제 없음
                        if (FuncNum == 0)
                        {
                            Products[i].Amount += _amount;
                            return;
                        }
                        //기존 제품 제거후 교체
                        else if (FuncNum == 1)
                        {
                            Products[i].product.productInfo.Name = _productName;
                            Products[i].product.productInfo.Price = _price;
                            Products[i].Amount = _amount;
                            Products[i].TotalSellAmount = 0;
                            return;
                        }
                        //같은 이름 다른 가격으로 저장
                        else if (FuncNum == 2)
                        {
                            break;
                        }
                        else if (FuncNum == 3)
                        {
                            return;
                        }
                    }
                }
            }
            // 품목 추가 가능한 공간이 있는지 확인
            // 있다면 새상품 추가
            for (int i = 0; i < Products.Length; i++)
            {
                if (Products[i].product == null)
                {
                    Products[i].product = new Product();
                    Products[i].product.productInfo.Name = _productName;
                    Products[i].Amount = _amount;
                    Products[i].product.productInfo.Price = _price;

                    return;
                }
            }
            Console.WriteLine("최대 저장 가능한 품목 갯수 초과 하였습니다.");
            Console.WriteLine("기존 제품 삭제후 다시 시도해주세요 ");
        }
        /// <summary>
        /// 상품추가시 기존의 상품들과 모호한 경우를 위함
        /// </summary>
        public int ProductCheck(string _name, int _price)
        {
            int IsRight = 0;
            for (int i = 0; i < Products.Length; i++)
            {
                if (Products[i].product != null)
                {
                    if (Products[i].product.productInfo.Name == _name)
                    {
                        if (Products[i].product.productInfo.Price != _price)
                        {
                            Console.WriteLine("같은 상품, 다른 가격이 발견되었습니다. 기존 상품을 제거하고 새로 추가 하시겠습니까?");
                            Console.WriteLine("1 : 제거 및 추가, 2: 계속 진행, 3: 종료");
                            IsRight = int.Parse(Console.ReadLine());

                            while (true)
                            {
                                if (IsRight == 1)
                                {
                                    return IsRight;
                                }
                                else if (IsRight == 2)
                                {
                                    return IsRight;
                                }
                                else if (IsRight == 3)
                                {
                                    return IsRight;
                                }
                            }
                        }
                    }
                }
            }

            return IsRight;
        }

        public void Add()
        {
            string ProductName = "None";
            int ProductPrice = 0;
            int ProductAmount = 0;
            bool NameIsRight = false;
            bool PriceIsRight = false;
            bool AmountIsRight = false;

            while (!NameIsRight)
            {
                Console.WriteLine("이름을 입력 해 주세요");
                string name = Console.ReadLine();
                Console.WriteLine(name + "맞습니까?");
                Console.WriteLine("맞으면 1, 아니면 2, 종료는 3을 눌러주세요");
                int isRight = int.Parse(Console.ReadLine());

                if (isRight == 1)
                {
                    NameIsRight = true;
                    ProductName = name;
                }
                else if (isRight == 2)
                {
                    NameIsRight = false;
                }
                else if (isRight == 3)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                    NameIsRight = false;
                }
            }
            while (!PriceIsRight)
            {
                Console.WriteLine("가격을 입력 해 주세요");
                int price = int.Parse(Console.ReadLine());
                Console.WriteLine(price + "원 맞습니까?");
                Console.WriteLine("맞으면 1, 아니면 2, 종료는 3을 눌러주세요");
                int isRight = int.Parse(Console.ReadLine());

                if (isRight == 1)
                {
                    PriceIsRight = true;
                    ProductPrice = price;
                }
                else if (isRight == 2)
                {
                    PriceIsRight = false;
                }
                else if (isRight == 3)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                    PriceIsRight = false;
                }

            }
            while (!AmountIsRight)
            {
                Console.WriteLine("수량을 입력해 주세요");
                int amount = int.Parse(Console.ReadLine());
                Console.WriteLine(amount + "개 맞습니까?");
                Console.WriteLine("맞으면 1, 아니면 2, 종료는 3을 눌러주세요");
                int isRight = int.Parse(Console.ReadLine());
                if (isRight == 1)
                {
                    AmountIsRight = true;
                    ProductAmount = amount;
                }
                else if (isRight == 2)
                {
                    AmountIsRight = false;
                }
                else if (isRight == 3)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                    AmountIsRight = false;
                }
            }
            FindOverride(ProductName, ProductAmount, ProductPrice);
        }//Add End

        /// <summary>
        /// 자판기에 잔돈을 충전
        /// </summary>
        public void InsertChanges()
        {
            Console.WriteLine("천 원 권 몇장 입력하시겠습니까?");
            Changes[1000] += int.Parse(Console.ReadLine());
            Console.WriteLine("오백원 권 몇개 입력하시겠습니까?");
            Changes[500] += int.Parse(Console.ReadLine());
            Console.WriteLine("백 원 권 몇개 입력하시겠습니까?");
            Changes[100] += int.Parse(Console.ReadLine());
            Console.WriteLine("오십원 권 몇개 입력하시겠습니까?");
            Changes[50] += int.Parse(Console.ReadLine());
            Console.WriteLine("십 원 권 몇개 입력하시겠습니까?");
            Changes[10] += int.Parse(Console.ReadLine());

            ShowChanges();
        }
        public void Sort(SellInfo[] sellInfos)
        {
            Console.WriteLine("분류 방법을 선택해주세요");
            Console.WriteLine("1 : 이름순 정렬");
            Console.WriteLine("2 : 가격순 정렬");
            Console.WriteLine("3 : 재고순 정렬");
            Console.WriteLine("4 : 종료");
            int num = int.Parse(Console.ReadLine());
            bool istrue = true;
            while (istrue)
            {
                istrue = false;
                if (num == 1)
                {
                    SortByName(sellInfos);
                }
                else if (num == 2)
                {
                    SortByPrice(sellInfos);
                }
                else if (num == 3)
                {
                    SortByStore(sellInfos);
                }
                else if (num == 4)
                {
                    return;
                }
                else
                {
                    istrue = true;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                }
            }
            Console.WriteLine();
            Console.WriteLine("분류 완료");
            Console.WriteLine();

            ShowProduct();
        }

        /// <summary>
        /// 이름으로 정렬
        /// </summary>
        public void SortByName(SellInfo[] sellInfos)
        {
            int[] names = new int[sellInfos.Length];
            int index = 0;

            for (int i = 0; i < sellInfos.Length; i++)
            {
                if (sellInfos[i].product != null)
                {
                    char[] arr = sellInfos[i].product.productInfo.Name.ToCharArray();
                    int value = 0;
                    for (int j = 0; j < arr.Length; j++)
                    {
                        value += Convert.ToInt32(arr[j]);
                    }
                    names[index] = value;
                }
                index++;
            }
            DeleteSort(names, sellInfos);
        }
        /// <summary>
        /// 가격으로 정렬
        /// </summary>
        public void SortByPrice(SellInfo[] sellInfos)
        {
            int[] price = new int[sellInfos.Length];
            for (int i = 0; i < sellInfos.Length; i++)
            {
                if (sellInfos[i].product != null)
                {
                    price[i] = sellInfos[i].product.productInfo.Price;
                }
            }

            DeleteSort(price, sellInfos);
        }
        /// <summary>
        /// 재고량으로 정렬
        /// </summary>
        public void SortByStore(SellInfo[] sellInfos)
        {
            int[] Amount = new int[sellInfos.Length];
            for (int i = 0; i < sellInfos.Length; i++)
            {
                if (sellInfos[i].product != null)
                {
                    Amount[i] = sellInfos[i].Amount;
                }
            }
            DeleteSort(Amount, sellInfos);
        }

        /// <summary>
        /// 이름으로 검색
        /// </summary>
        public void FindByName(string name)
        {
            Console.WriteLine("이름으로 검색합니다");
            bool isFind = true;
            for (int i = 0; i < Products.Length; i++)
            {
                if (Products[i].product != null)
                {
                    if (Products[i].product.productInfo.Name == name)
                    {
                        Console.WriteLine("검색 된 상품번호 {0}", i + 1 + "번 : " + name);
                        Console.WriteLine("검색 된 상품 : " + name);
                        Console.WriteLine("검색 된 상품가격 : " + Products[i].product.productInfo.Price + "원");
                        Console.WriteLine("검색 된 상품 재고 : " + Products[i].Amount + "개");
                        isFind = false;
                    }
                }
            }
            if (isFind)
            {
                Console.WriteLine("검색 된 상품 없음");
            }
        }
        /// <summary>
        /// 가격으로 검색
        /// </summary>
        public void FindByPrice(int price)
        {
            Console.WriteLine("가격으로 검색합니다");
            bool isFind = true;
            for (int i = 0; i < Products.Length; i++)
            {
                if (Products[i].product != null)
                {
                    if (Products[i].product.productInfo.Price == price)
                    {
                        Console.WriteLine("검색 된 상품번호 {0}", i + 1 + "번 : " + Products[i].product.productInfo.Name);
                        Console.WriteLine("검색 된 상품 : " + Products[i].product.productInfo.Name);
                        Console.WriteLine("검색 된 상품가격 : " + price + "원");
                        Console.WriteLine("검색 된 상품 재고 : " + Products[i].Amount + "개");
                        isFind = false;
                    }
                }
            }
            if (isFind)
            {
                Console.WriteLine("검색 된 상품 없음");
            }

        }
        /// <summary>
        /// 이름과 가격으로 조회
        /// </summary>
        public void FindByNameAndPrice(string name, int price)
        {
            Console.WriteLine("이름과 가격으로 검색합니다");
            bool isFind = true;
            for (int i = 0; i < Products.Length; i++)
            {
                if (Products[i].product != null)
                {
                    if (Products[i].product.productInfo.Price == price && Products[i].product.productInfo.Name == name)
                    {
                        if (Products[i].product.productInfo.Price == price)
                        {
                            Console.WriteLine("검색 된 상품번호 {0}", i + 1 + "번 : " + Products[i].product.productInfo.Name);
                            Console.WriteLine("검색 된 상품 : " + Products[i].product.productInfo.Name);
                            Console.WriteLine("검색 된 상품가격 : " + price + "원");
                            Console.WriteLine("검색 된 상품 재고 : " + Products[i].Amount + "개");
                            isFind = false;
                        }
                    }
                }
            }
            if (isFind)
            {
                Console.WriteLine("검색 된 상품 없음");
            }

        }
        public void ShowProduct()
        {
            int index = 1;
            foreach (SellInfo sellInfo in Products)
            {
                if (sellInfo.product != null)
                {
                    if (sellInfo.Amount > 0)
                    {
                        Console.WriteLine("{0}번 상품명 : " + sellInfo.product.productInfo.Name +"  가격 : {1}원",index, sellInfo.product.productInfo.Price);
                    }
                    else
                    {
                        Console.WriteLine("품절... {0}번 상품명 : " + sellInfo.product.productInfo.Name, index);
                    }
                }
                else
                {
                    Console.WriteLine("{0}번 상품 없음", index);
                }
                index++;
            }
        }

        public void Find()
        {
            Console.WriteLine("상품 검색");
            Console.WriteLine();
            Console.WriteLine("1 : 이름 검색, 2 : 가격 검색, 3 : 이름과 가격으로 검색, 4 : 종료");
            Console.WriteLine();

            int FuncNum = int.Parse(Console.ReadLine());
            bool Istrue = false;
            while (!Istrue)
            {
                if (FuncNum == 1)
                {
                    Console.WriteLine("상품명을 입력해주세요");
                    string Name = Console.ReadLine();
                    FindByName(Name);
                    Istrue = true;
                }
                else if (FuncNum == 2)
                {
                    Console.WriteLine("상품 가격을 입력해주세요");
                    int Price = int.Parse(Console.ReadLine());
                    FindByPrice(Price);
                    Istrue = true;
                }
                else if (FuncNum == 3)
                {
                    Console.WriteLine("상품명을 입력해주세요");
                    string Name = Console.ReadLine();
                    Console.WriteLine("상품 가격을 입력해주세요");
                    int Price = int.Parse(Console.ReadLine());
                    FindByNameAndPrice(Name, Price);
                    Istrue = true;
                }
                else if (FuncNum == 4)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("1 : 이름 검색, 2 : 가격 검색, 3 : 이름과 가격으로 검색, 4 : 종료");
                    Console.WriteLine("잘못된 입력 다시 입력 해 주세요");
                }
            }
        }
        public void RemoveProduct(int Number)
        {
            Remove(Number);
            Console.WriteLine();
            Console.WriteLine("{0}", Number + 1 + " 번 제거 완료");
            Console.WriteLine();
        }
        public void Remove(int num)
        {
            for(int i = 0; i < Products.Length; i++)
            {
                //제거후 사라진 자리에 대한 정렬도 시행함
                if(Products[i].product ==null)
                {
                    Products[num].TotalSellAmount = Products[i - 1].TotalSellAmount;
                    Products[num].Amount = Products[i - 1].Amount;
                    Products[num].product = Products[i - 1].product;
                    Products[i - 1].product = null;
                    Products[i - 1].TotalSellAmount = 0;
                    Products[i - 1].Amount = 0;
                    break;
                }
            }




        }
        public void SelectProduct()
        {
            Console.WriteLine();
            ShowProduct();
            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("현재 잔액 : {0}원.", Cash);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("상품을 입력해주세요");
                int num = int.Parse(Console.ReadLine()) - 1;

                //재고량이 충분한지 확인
                if (Products[num].Amount > 0)
                {
                    if (Products[num].product.productInfo.Price <= Cash)
                    {
                        string name = Products[num].product.productInfo.Name;
                        Console.WriteLine("{0} 상품 구매 완료", name);
                        Cash -= Products[num].product.productInfo.Price;
                        Products[num].Amount -= 1;
                        SaveSellInfo(1, Products[num].product);
                    }
                    else
                    {
                        Console.WriteLine("잔액이 부족합니다");
                        bool isover = false;
                        while (!isover)
                        {
                            Console.WriteLine("현금을 더 넣으시겠습니까?");
                            Console.WriteLine("1 : 현금 충전");
                            Console.WriteLine("2 : 뒤로가기");
                            int choise = int.Parse(Console.ReadLine());
                            if (choise == 1)
                            {
                                Console.WriteLine("현금을 넣어주세요");
                                int _cash = int.Parse(Console.ReadLine());
                                InsertCash(_cash);
                                isover = true;
                            }
                            else if (choise == 2)
                            {
                                isover = true;
                            }
                            else
                            {
                                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                            }
                        }
                    }
                    Console.WriteLine();
                    ShowCashes();
                    Console.WriteLine();

                    break;
                }
                else
                {
                    Console.WriteLine("해당 상품은 품절입니다. 다른 상품을 골라 주세요");
                    return;
                }
            }

        }
        public void ShowCashes()
        {
            Console.WriteLine("현재 잔액 : {0}원", Cash);
        }
        public void ShowChanges()
        {            
            Console.WriteLine("천 원 권 : " + Changes[1000] + " 매");
            Console.WriteLine("오백원 권 : " + Changes[500] + " 개");
            Console.WriteLine("백 원 권 : " + Changes[100] + " 개");
            Console.WriteLine("오십원 권 : " + Changes[50] + " 개");
            Console.WriteLine("십 원 권 : " + Changes[10] + " 개");
            Console.WriteLine();
            int total = 0;
            foreach (int Unit in CoinUnits)
            {
                total += Unit * Changes[Unit];
            }
            TotalChanges = total;
            Console.WriteLine("현재 총 잔액 : " + total + "원");
        }

        /// <summary>
        /// 상품 구매에 대한 기록
        /// </summary>
        private void SaveSellInfo(int amount, Product _product)
        {
            for (int i = 0; i < Products.Length; i++)
            {
                if (Products[i].product != null)
                {
                    if (Products[i].product.productInfo.Name == _product.productInfo.Name
                        && Products[i].product.productInfo.Price == _product.productInfo.Price)
                    {
                        Products[i].TotalSellAmount += amount;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 상품 판매 내역
        /// </summary>
        public void ShowTotalSales()
        {
            int total = 0;
            int index = 0;
            foreach (SellInfo sellInfo in Products)
            {
                if (Products[index].product != null)
                {
                    if (Products[index].product.productInfo.Name != null)
                    {
                        Console.WriteLine("상품 명 : " + sellInfo.product.productInfo.Name);
                        Console.WriteLine("가격 : " + sellInfo.product.productInfo.Price + "원");
                        Console.WriteLine("총 판매 갯수 : " + sellInfo.TotalSellAmount + "개");
                        Console.WriteLine("현재 재고 : " + sellInfo.Amount + "개");
                        Console.WriteLine("해당 상품 총 판매 액 : " + sellInfo.TotalSellAmount * sellInfo.product.productInfo.Price + "원");
                        total += sellInfo.TotalSellAmount * sellInfo.product.productInfo.Price;
                    }
                }
                index++;
            }
            Console.WriteLine("전체 제품 판매액 " + total + "원");
        }

        public void DeleteSort(int[] arr, SellInfo[] sellInfos)
        {
            int[] replica = new int[arr.Length]; //출력용 배열
            int[] copy = new int[arr.Length]; //원본 유지를 위한, 임시저장 배열
            int LowLimit = 5; //기준점을 잡기위한 덤프값임, 아무값이나 넣어도 상관없음

            //최소값을 찾기 위함, 시작점을 만들기 위한 단계
            for (int i = 0; i < copy.Length; i++)
            {
                copy[i] = arr[i];
                if (copy[i] < LowLimit)
                {
                    LowLimit = copy[i];
                }
            }


            for (int i = 0; i < copy.Length; i++)
            {
                int current = LowLimit;
                //J열에서 가장 큰 순서대로 뽑아서 앞으로 밀어넣음
                for (int j = 0; j < copy.Length; j++)
                {
                    if (current <= copy[j])
                    {
                        current = copy[j];
                    }

                }
                //J열에서 나온 값을 찾아서 소거함
                for (int h = 0; h < copy.Length; h++)
                {
                    if (current == copy[h])
                    {
                        copy[h] = LowLimit;
                        break;
                    }
                }
                replica[i] = current;
            }

            //정보 교환
            for (int i = 0; i < sellInfos.Length; i++)
            {
                for (int j = i; j < sellInfos.Length; j++)
                {
                    if (sellInfos[j].product != null)
                    {
                        if (replica[i] == arr[j])
                        {
                                //under Product
                                string SaveName = sellInfos[i].product.productInfo.Name;
                                int SavePrice = sellInfos[i].product.productInfo.Price;

                                //under Sellinfo
                                int SaveTotalSell = sellInfos[i].TotalSellAmount;
                                int SaveAmount = sellInfos[i].Amount;

                                //under Change Infos
                                sellInfos[i].product.productInfo.Name = sellInfos[j].product.productInfo.Name;
                                sellInfos[i].product.productInfo.Price = sellInfos[j].product.productInfo.Price;

                                sellInfos[i].Amount = sellInfos[j].Amount;
                                sellInfos[i].TotalSellAmount = sellInfos[j].TotalSellAmount;

                                sellInfos[j].product.productInfo.Name = SaveName;
                                sellInfos[j].product.productInfo.Price = SavePrice;

                                sellInfos[j].TotalSellAmount = SaveTotalSell;
                                sellInfos[j].Amount = SaveAmount;
                                break;                           
                        }
                    }
                }
            }
        }
    }
}
