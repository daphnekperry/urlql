# urlql

Putting the query back in query string.
REsT.. for the rest of us.

Setup (the simple version)
=====

Add the QueryArguments ModelBinder
```
services.AddMvc(options =>
{
    options.ModelBinderProviders.Insert(0, new urlql.asp.core.QueryArgumentsBinderProvider());
})
```

Add the QueryOptions to your DI Container.  Resist the urge to do this as a singleton so that have the option to can apply query options attributes to your controller actions and not modify your singleton instance.
```
services.AddTransient<QueryOptions>();
```

Add the QueryArgumentsValidation to your get method and pass in the QueryArguments and QueryOptions as shown.
Pass your desired IQueryable along with the arguments and options to a new resolver.
Calling GetIActionResultAsync() will provide either an Ok 200 response containing a page result of the query or Bad Request 400 response if they arguments fail validation.
Enjoy easy querying of your desired IQueryable.
```
[HttpGet()]
[QueryArgumentsValidation]
public async Task<IActionResult> Get(QueryArguments arguments, [FromServices] QueryOptions options)
{
    var events = repo.Calendar.Events;
    var resolver = new QueryResolver(events, arguments ?? new QueryArguments(), options);
    var result = await resolver.GetIActionResultAsync();
    return result;
}
```

Usage
=====
Here are some examples:
```
/api/entity?skip=0&take=100&order=id desc
/api/entity?select=cnt as Count
/api/entity?select=ownerUserName as User, cdt id as total&group=User&order=total desc
```

The ordering of keyword expressions does not matter, such as skip={}&select={}&take={} is valid.

Paging
------
Query String keyword(s):
?skip={value}&take={value}

In most cases, skip and take are optional values but when provided must be provided together, you cannot skip without a take and vice versa.
|Keyword|Usage|
|--|--|
|Skip|Start index (0 based) from where to pull pages|
|Take|Total number of records to retrieve|


Selection
------
Query String keyword(s):
?select={expression}

Statements:
[attribute]
[attribute] as [Alias]
[aggregation] ?[attribute] as [Alias]

Separate statements by commas.  Attributes are type checked against the target entity and only value types (strings, numbers) can be used in the selection statement.  Selected attributes will be returned with the attribute name on the entity by default.  Selected values can be aliased and all aggregations require that an alias be provided.

|Aggregations|Description|
|--|--|
|cnt|Total count of all entities|
|dct|Distinct count of values of a property|
|min|Minimum value of a property|
|max|Maximum value of a property|
|avg|Average value of a property|
|sum|Sum of all values of a property|

Depending on the content of the expression when using an aggregation, a grouping expression may be required.

Filtering
---------
Query String keyword(s):
?filter={expression}
?where={expression}

Statements:
[property] [keyword] [value]

The keywords 'and' and 'or' chain together multiple conditionals.
```
id gt 0 and isDeleted ne true
```
```
accountId eq 84 or accountId eq 22
```

Parenthesis can be used to create complex conditional expressions or sub-expressions.
```
(startDate ge "2019-08-01" and startDate lt "2019-08-02") or isCompleted eq false
```

The following keywords can be used in constructing a filtering expression.
|Keyword|Description|
|--|--|
|eq|equals|
|ne|not equals|
|lt|less than|
|gt|greater than|
|le|less than or equal to|
|ge|greater than or equal to|
|cn|contains text|
|nc|does not contain text|
|st|starts with text|
|ed|ends with text|
|ieq|case insensitive equals|
|ine|case insensitive not equals|
|icn|case insensitive contains|
|inc|case insensitive does not contain|
|ist|case insensitive starts with|
|ied|case insensitive ends with|

Ordering
--------
Query String keywords(s):
?order={expression}
?orderby={expression}

[property|alias] [asc|desc]

Multiple order statements can be provided and are are separated by commas.
```
ownerId desc, id desc
```

|Keyword|Description|
|--|--|
|asc|ascending order|
|desc|descending order|

Grouping
--------
Query String keywords(s):
?group={expression}
?groupby={expression}

[property|alias]

Multiple group statements can be provided and are separated by commas.
```
ownerId, accountId
```















