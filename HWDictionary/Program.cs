using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace HWDictionary
{
    /*
    * Задание 1. 
    Создать примитивный англо-русский и русско-английский словарь, 
    содержащий пары слов — названий стран на русском и английском языках. 
    Пользователь должен иметь возможность выбирать направление перевода и запрашивать перевод.   
    */

    class EnglishRussianDictionary
    {
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();
        private Regex regex;

        public EnglishRussianDictionary(Dictionary<string, string> dictionary) 
        { 
            this.dictionary = dictionary;
        }

        public string Translate(string word)
        {
            this.regex = new Regex("[a-z]", RegexOptions.IgnoreCase); // тут через регулярные выражения сделал, а ниже, в Remove(), - через коды символов по ASCII
            if (this.regex.IsMatch(word))
            {
                if (this.dictionary.TryGetValue(word, out string rusWord))
                {
                    return rusWord;
                }
                return "перевод не найден";
            }

            this.regex = new Regex("[а-я]", RegexOptions.IgnoreCase);
            if (this.regex.IsMatch(word))
            {
                foreach (var w in this.dictionary)
                {
                    if (w.Value == word)
                    {
                        return w.Key;
                    }
                }
            }
            return "перевод не найден";
        }

        public void Add(string engWord, string rusWord) 
        {
            if (!this.dictionary.TryAdd(engWord, rusWord))
            {
                Console.WriteLine($"Слово {engWord} ({rusWord}) уже есть в словаре");
            }
            
        }

        public void Remove(string word)
        {
            if (word[0] <= 122) // английские буквы находятся в диапазоне от 65 до 90 - заглавные и 97 до 122 - строчные
            {
                if (!this.dictionary.Remove(word))
                {
                    Console.WriteLine($"Слово {word} не найдено в словаре для удаления");
                }
            }
            else if (word[0] >= 1040) // русские буквы от 1040
            {
                foreach (var w in this.dictionary)
                {
                    if (w.Value == word)
                    {
                        this.dictionary.Remove(w.Key);
                    }
                }
            }
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public void Print()
        {
            Console.WriteLine("==================================");
            Console.WriteLine("=      АНГЛО-РУССКИЙ СЛОВАРЬ     =");
            Console.WriteLine("==================================");
            if (this.dictionary.Count != 0)
            {
                foreach (var word in this.dictionary)
                {
                    Console.WriteLine($"| {word.Key} {(word.Key.Length >= 5 ? "\t" : "\t\t")} | {word.Value} {(word.Value.Length <= 3 ? "\t\t" : "\t")} |");
                }
                Console.WriteLine("==================================");
            }
            else
            {
                Console.WriteLine("|          СЛОВАРЬ ПУСТ          |");
                Console.WriteLine("==================================");
            }
        }
    }



    /*
    * Задание 2.
    Подсчитать, сколько раз каждое слово встречается в заданном тексте. 
    Результат записать в коллекцию Dictionary<TKey, TValue>.
    */
    class WordCountInText
    {
        string text;
        Dictionary<string, ulong> wordCount;

        public WordCountInText(string text)
        {
            this.text = text;
            this.wordCount = new Dictionary<string, ulong>();
        }

        public void Count(string word, bool ignoreCase = false)
        {
            if (this.wordCount.ContainsKey(word))
            {
                Console.WriteLine($"Cлово \"{word}\" уже было подсчитано!\n");
                return;
            }

            ulong count = 0;

            if (ignoreCase == false)
            {
                foreach (string w in this.text.Split(' ', '.', ',', ':', ';', '!', '?', '(', ')', '№', '$', '\t', '\n', '\t', '\b'))
                {
                    if (w == word)
                    {
                        count++;
                    }
                }
            }
            else
            {
                foreach (string w in this.text.ToLower().Split(' ', '.', ',', ':', ';', '!', '?', '(', ')', '№', '$', '\t', '\n', '\t', '\b'))
                {
                    if (w == word)
                    {
                        count++;
                    }
                }
                word = "[" + word + "]"; // специальный вид для тех слов, которые считались с ignoreCase
            }

            this.wordCount.Add(word, count);
        }

        public void Result(string word = null)
        {
            if (word == null)
            {
                Console.WriteLine("Кол-во повторений слов в тексте: ");
                foreach (var wc in this.wordCount)
                {
                    Console.WriteLine($"\"{wc.Key}\"\t\t{wc.Value}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Кол-во повторений слова \"{word}\" в тексте: {this.wordCount[word]}");
            }
        }
    }



    internal class Program
    {
        static void Main()
        {


        // ЗАДАНИЕ 1
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                ["Apple"] = "Яблоко",
                ["Bee"] = "Пчела",
                ["Cat"] = "Кошка",
                ["Dolphin"] = "Дельфин",
                ["Elephant"] = "Слон",
                ["Fish"] = "Рыба",
                ["Giraffe"] = "Жираф",
                ["House"] = "Здание",
                ["Ice"] = "Лёд",
                ["Jelly"] = "Желе",
                ["Key"] = "Ключ",
                ["Love"] = "Любовь",
                ["Mouse"] = "Мышь",
                ["Newt"] = "Ящерица",
                ["Octopus"] = "Осьминог",
                ["Pig"] = "Свинья",
                ["Queen"] = "Принцесса",
                ["Rocket"] = "Ракета",
                ["Snake"] = "Змея",
                ["Tree"] = "Дерево",
                ["Umbrella"] = "Зонтик",
                ["Vase"] = "Ваза",
                ["Whale"] = "Кит",
                ["Xylophone"] = "Ксилофон",
                ["Yo-yo"] = "Йо-йо",
                ["Zebra"] = "Зебра"
            };


            EnglishRussianDictionary engRusDict = new EnglishRussianDictionary(dictionary);

            engRusDict.Print(); Console.WriteLine("\n");


            // ПРОВЕРКА Translate();
            // английские слова
            string engWord1 = "Apple";
            Console.WriteLine($"Перевод слова {engWord1}: {engRusDict.Translate(engWord1)}");

            string engWord2 = "Pig";
            Console.WriteLine($"Перевод слова {engWord2}: {engRusDict.Translate(engWord2)}");

            string engWord3 = "Banana";
            Console.WriteLine($"Перевод слова {engWord3}: {engRusDict.Translate(engWord3)}\n");

            // русские слова
            string rusWord1 = "Яблоко";
            Console.WriteLine($"Перевод слова {rusWord1}: {engRusDict.Translate(rusWord1)}");

            string rusWord2 = "Свинья";
            Console.WriteLine($"Перевод слова {rusWord2}: {engRusDict.Translate(rusWord2)}");

            string rusWord3 = "Банан";
            Console.WriteLine($"Перевод слова {rusWord3}: {engRusDict.Translate(rusWord3)}\n\n");


            // ПРОВЕРКА Add();
            string engWordForAdd = "Banana";
            string rusWordForAdd = "Банан";

            engRusDict.Add("Zebra", "Зебра"); // попытка добавить уже имеющееся слово
            engRusDict.Add(engWordForAdd, rusWordForAdd);
            Console.WriteLine($"Перевод слова {engWordForAdd} после его добавления: {engRusDict.Translate(engWordForAdd)}\n\n");

            //engRusDict.Print(); Console.WriteLine("\n");


            // ПРОВЕРКА Remove();
            string engWordForRem = "Banana";
            string rusWordForRem = "Яблоко";

            engRusDict.Remove("Home"); // ошибка, так как слова в словаре нет
            engRusDict.Remove(engWordForRem);
            engRusDict.Remove(rusWordForRem);

            Console.WriteLine($"Перевод слова {engWordForRem} после его удаления: {engRusDict.Translate(engWordForRem)}");
            Console.WriteLine($"Перевод слова {rusWordForRem} после его удаления: {engRusDict.Translate(rusWordForRem)}\n\n");

            //engRusDict.Print(); Console.WriteLine("\n");


            // ПРОВЕРКА Clear();
            engRusDict.Clear();

            engRusDict.Print(); Console.WriteLine("\n");







    // ЗАДАНИЕ 2
            string text = // текст я попросил CharGPT написать
            "Я без ума от старого дома в лесу, который мне удалось арендовать на выходные. " +
            "Но когда наступила ночь, я начал слышать звуки, которые казались мне странными. " +
            "Шаги на полу, скрип дверей, тихое дыхание. " +
            "Я попытался проигнорировать их и заснуть, но когда я проснулся на утро, я увидел, что кровать, на которой я спал, была покрыта кровью. " +
            "Я испугался и попытался уйти, но дверь, которая раньше была открыта, теперь была заперта изнутри." +
            "Я понял, что я не один в этом доме. И тогда я услышал голос, который сказал: \"Ты не должен был здесь останавливаться\". " +
            "Я обернулся, но никого не было." +
            "Я быстро вышел из комнаты и пошел по коридору, но всякий раз, когда я пытался открыть дверь, я обнаруживал, что она заперта. " +
            "Я понял, что я попал в ловушку. Я был здесь не один и тот, кто здесь был раньше, не ушел живым." +
            "Я услышал шаги, которые приближались, и понял, что мне придется бороться за свою жизнь. " +
            "Я схватил любую вещь, которую мог найти, и ждал. Но когда я нашел того, кто был здесь раньше, я понял, что он был уже мертв. Я был следующим.";

            WordCountInText wordCountInText = new WordCountInText(text);

            wordCountInText.Count("дверь");
            wordCountInText.Count("я", true);
            wordCountInText.Count("я");
            wordCountInText.Count("я"); // не получится
            wordCountInText.Count("мертв");

            wordCountInText.Result(); // общий вывод
            wordCountInText.Result("я"); // вывод для конкретного слова
        }
    }
}