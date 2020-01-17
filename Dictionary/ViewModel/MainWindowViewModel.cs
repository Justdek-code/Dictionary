﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dictionary.Model;

namespace Dictionary.ViewModel
{
    // Enum that represents part of speech
    public enum PartSpeech
    {
        Unknown,
        Noun,
        Pronoun,
        Verb,
        Adjective,
        Adverb,
        Preposition,
        Conjunction,
        Articles,
        Interjection
    }
    class MainWindowViewModel
    {
        public SettingsModel settings;
        public DataBaseAccess dataAccess;

        // Fields that receive an input
        public string Word { get; set; }
        public string Definition { get; set; }
        public PartSpeech Part { get; set; }

        // Buttons interaction interface
        public Command AddButton { get; set; }
        public Command DelButton { get; set; }
        public Command EditButton { get; set; }

        // Binging collection for DataGrid
        public ObservableCollection<WordModel> WordCollection { get; set; }

        // Selected item in DataGrid
        public WordModel SelectedWord { get; set; }

        public MainWindowViewModel()
        {   
            AddButton = new Command(AddAction);
            DelButton = new Command(DelAction);
            EditButton = new Command(EditAction);

            settings = SettingsModel.GetInstance();
            dataAccess = DataBaseAccess.GetInstance();
            WordCollection = dataAccess.LoadWords();
        }

        // Action when user click Add button
        private void AddAction()
        {
            WordModel word = new WordModel(Word, Definition, Part);
            settings.WordsAmount += 1;
            WordCollection.Add(word);
            dataAccess.SaveWord(word);
        }

        // Action when user click Del button
        private void DelAction()
        {
            if (SelectedWord != null)
            {
                settings.WordsAmount -= 1;
                dataAccess.RemoveWord(SelectedWord);
                WordCollection.Remove(SelectedWord);
            }
            
            
        }

        // Action when user click Edit button
        private void EditAction()
        {

        }
    }
}
 