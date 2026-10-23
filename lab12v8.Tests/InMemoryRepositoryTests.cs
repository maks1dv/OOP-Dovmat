using System;
using System.Collections.Generic;
using System.Linq;
using lab12v8;
using Xunit;

namespace lab12v8.Tests
{
    public class InMemoryRepositoryTests
    {
        private readonly InMemoryRepository<TaskItem> _repository;

        public InMemoryRepositoryTests()
        {
            _repository = new InMemoryRepository<TaskItem>();
        }

        [Fact]
        public void Add_ShouldAddItemToRepository()
        {
            var task = new TaskItem { Id = 1, Title = "Тест", Description = "Опис", IsCompleted = false };
            _repository.Add(task);
            var result = _repository.GetById(1);
            Assert.NotNull(result);
            Assert.Equal("Тест", result.Title);
        }

        [Fact]
        public void Add_DuplicateId_ShouldThrowInvalidOperationException()
        {
            var task1 = new TaskItem { Id = 1, Title = "Тест 1" };
            var task2 = new TaskItem { Id = 1, Title = "Дублікат" };
            _repository.Add(task1);
            Assert.Throws<InvalidOperationException>(() => _repository.Add(task2));
        }

        [Fact]
        public void GetById_ExistingId_ShouldReturnItem()
        {
            var task = new TaskItem { Id = 5, Title = "Пошук" };
            _repository.Add(task);
            var result = _repository.GetById(5);
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
        }

        [Fact]
        public void GetById_NonExistingId_ShouldReturnNull()
        {
            var result = _repository.GetById(999);
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllItems()
        {
            _repository.Add(new TaskItem { Id = 1, Title = "Завдання 1" });
            _repository.Add(new TaskItem { Id = 2, Title = "Завдання 2" });
            var items = _repository.GetAll();
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public void Update_ExistingItem_ShouldUpdateData()
        {
            var task = new TaskItem { Id = 1, Title = "Старий заголовок", IsCompleted = false };
            _repository.Add(task);
            var updatedTask = new TaskItem { Id = 1, Title = "Новий заголовок", IsCompleted = true };
            _repository.Update(updatedTask);
            var result = _repository.GetById(1);
            Assert.NotNull(result);
            Assert.Equal("Новий заголовок", result.Title);
            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void Update_NonExistingItem_ShouldThrowKeyNotFoundException()
        {
            var task = new TaskItem { Id = 100, Title = "Неіснуючий" };
            Assert.Throws<KeyNotFoundException>(() => _repository.Update(task));
        }

        [Fact]
        public void Delete_ExistingId_ShouldRemoveItem()
        {
            var task = new TaskItem { Id = 1, Title = "Для видалення" };
            _repository.Add(task);
            _repository.Delete(1);
            Assert.Null(_repository.GetById(1));
        }

        [Fact]
        public void Delete_NonExistingId_ShouldThrowKeyNotFoundException()
        {
            Assert.Throws<KeyNotFoundException>(() => _repository.Delete(999));
        }
    }
}
