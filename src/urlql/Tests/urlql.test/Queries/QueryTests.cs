using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlql.Expressions;

namespace urlql.test.Queries
{
    public class QueryTests : Specificaton
    {
        public IEnumerable<Foo> objects;
        public QueryEngine query;
        public QueryOptions options;
        public QueryArguments arguments;
        public QueryResult results;

        protected override void Arrange()
        {
            objects = Foo.MakeFoo(42);
        }
    }

    public class SelectItems : QueryTests
    {
        protected override void Arrange()
        {
            base.Arrange();
            options = new QueryOptions();
            arguments = new QueryArguments();
            arguments.Selections = new List<ISelectionStatement>() { new Selection("id") };
            query = new QueryEngine(objects.AsQueryable(), arguments, options);
        }

        protected override void Act()
        {
            results = query.GetResultsAsync().Result;
        }

        [Test]
        public void ContainsId()
        {
            Assert.AreEqual(results.Count(), objects.Count());
            dynamic r = results.ToList();
        }
    }

    public class FilterItems : QueryTests
    {
        public int id = 3;
        protected override void Arrange()
        {
            base.Arrange();
            options = new QueryOptions();
            arguments = new QueryArguments();
            arguments.Filtering = new List<IFilteringStatement>() { new Comparison(ComparisonOperation.eq, "id", id.ToString()) };
            query = new QueryEngine(objects.AsQueryable(), arguments, options);
        }

        protected override void Act()
        {
            results = query.GetResultsAsync().Result;
        }

        [Test]
        public void ContainsRecordThree()
        {
            Assert.AreEqual(results.Count(), 1);
            dynamic r = results.ToList().FirstOrDefault();
            Assert.AreEqual(r.Id, id);
        }
    }

    public class OrderItems : QueryTests
    {
        protected override void Arrange()
        {
            base.Arrange();
            options = new QueryOptions();
            arguments = new QueryArguments();
            arguments.Ordering = new List<IOrderingStatement>() { new Ordering("id", OrderingOperation.desc) };
            query = new QueryEngine(objects.AsQueryable(), arguments, options);
        }

        protected override void Act()
        {
            results = query.GetResultsAsync().Result;
        }

        [Test]
        public void ObjectsAreOrdered()
        {
            Assert.AreEqual(results.Count(), objects.Count());
            var f = results.FirstOrDefault() as Foo;
            Assert.AreEqual(f.Id, objects.Max(s => s.Id));
            var l = results.LastOrDefault() as Foo;
            Assert.AreEqual(l.Id, objects.Min(s => s.Id));
        }
    }

    public class AggregateSelection : QueryTests
    {
        protected override void Arrange()
        {
            base.Arrange();
            options = new QueryOptions();
            arguments = new QueryArguments();
            arguments.Selections = new List<ISelectionStatement>() { new Aggregation(AggregationOperation.max, "id", new Alias("id", "maxId")) };
            query = new QueryEngine(objects.AsQueryable(), arguments, options);
        }

        protected override void Act()
        {
            results = query.GetResultsAsync().Result;
        }

        [Test]
        public void ObjectsAreAggregated()
        {
            Assert.AreEqual(results.Count(), 1);
            dynamic r = results.FirstOrDefault();
            Assert.AreEqual(r.maxId, 41);

        }
    }

    public class GroupSelection : QueryTests
    {
        protected override void Arrange()
        {
            //base.Arrange();
            options = new QueryOptions();
            arguments = new QueryArguments();
            objects = Foo.MakeFoo(44);
            objects.ToList().ForEach(o => o.CountLong = o.Id % 7);
            arguments.Selections = new List<ISelectionStatement>()
            {
                new Selection("CountLong"),
                new Aggregation(AggregationOperation.dct, "id", new Alias("id", "idCount"))
            };
            arguments.Grouping = new List<IGroupingStatement>()
            {
                new Grouping("CountLong")
            };
            query = new QueryEngine(objects.AsQueryable(), arguments, options);
        }

        protected override void Act()
        {
            results = query.GetResultsAsync().Result;
        }

        [Test]
        public void ObjectsAreGrouped()
        {
            Assert.AreEqual(results.Count(), 7);
            dynamic r = results.FirstOrDefault();
            Assert.AreEqual(r.idCount, 7);
            r = results.LastOrDefault();
            Assert.AreEqual(r.idCount, 6);
        }
    }

    public class PagingItems : QueryTests
    {
        protected override void Arrange()
        {
            base.Arrange();
            options = new QueryOptions();
            arguments = new QueryArguments();
            arguments.Paging = new Paging(5, 5);
            query = new QueryEngine(objects.AsQueryable(), arguments, options);
        }

        protected override void Act()
        {
            results = query.GetResultsAsync().Result;
        }

        [Test]
        public void TakeFive()
        {
            //Assert.Brubeck
            Assert.AreEqual(results.Count(), arguments.Paging.Take);
            Assert.AreEqual(results.EndRow, (arguments.Paging.Take + arguments.Paging.Skip) - 1);
        }

        [Test]
        public void StartedAtFive()
        {
            Assert.AreEqual(results.StartRow, arguments.Paging.Skip);
        }

        [Test]
        public void PageResultsCorrect()
        {
            Assert.AreEqual(results.IsValidResult, true);
            Assert.AreEqual(results.IsPagedResult, true);
            Assert.AreEqual(results.IsLastPage, false);

            var index = arguments.Paging.Skip;
            foreach (var r in results)
            {
                Assert.AreEqual((r as Foo).Id, this.objects.ElementAtOrDefault(index++).Id);
            }
        }
    }
}
