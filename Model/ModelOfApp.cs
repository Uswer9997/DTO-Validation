using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Model
{
    public class ModelOfApp
    {
        public void Save(string name, DateTime dateOfBirth)
        {
            MessageBox.Show($"Сохранение {name} - {dateOfBirth}.");
            people.Add(new Person(name, dateOfBirth));
        }


        // Локальный кеш. В данном случае просто наблюдаемая коллекция.
        // В реале может быть достаточно сложная логика.
        private readonly ObservableCollection<Person> people = new ObservableCollection<Person>();



        // Получение локального кеша через наблюдаемую коллекцию.
        public ReadOnlyObservableCollection<Person> GetPeople() =>
            _peop ?? (_peop = new ReadOnlyObservableCollection<Person>(people));
        private ReadOnlyObservableCollection<Person> _peop;

    }
}