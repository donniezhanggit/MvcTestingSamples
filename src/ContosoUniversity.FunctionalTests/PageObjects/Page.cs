﻿using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium;

namespace ContosoUniversity.FunctionalTests.PageObjects
{
    public class Page<TModel> : Page where TModel : new()
    {
        public Page<TModel> TextBoxFor<TField>(Expression<Func<TModel, TField>> field, TField value)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            var element = Host.Browser.FindElement(By.Name(name));
            element.Clear();
            element.SendKeys(value.ToString());

            return this;
        }

        public Page<TModel> InputModel(TModel model)
        {
            var type = model.GetType();
            foreach (var property in type.GetProperties())
            {
                var element = Host.Browser.FindElement(By.Name(property.Name));
                element.Clear();
                element.SendKeys(property.GetValue(model).ToString());
            }
            return this;
        }

        public string DisplayFor<TField>(Expression<Func<TModel, TField>> field)
        {
            string name = ExpressionHelper.GetExpressionText(field);
            string id = TagBuilder.CreateSanitizedId(name);

            var span = Host.Browser.FindElement(By.Id(id));

            return span.Text;
        }

        public TModel ReadModel()
        {
            var type = typeof(TModel);
            var instance = new TModel();

            foreach (var property in type.GetProperties())
            {
                string name = ExpressionHelper.GetExpressionText(property.Name);
                string id = TagBuilder.CreateSanitizedId(name);

                var span = Host.Browser.FindElement(By.Id(id));
                property.SetValue(instance, span.Text, null);
            }

            return instance;
        }
    }

    public abstract class Page
    {
        public string Title
        {
            get { return Host.Browser.Title; }
        }

        public string Url { get { return Host.Browser.Url; } }

        public string PageId
        {
            get
            {
                return Host.Browser.FindElement(By.Name("pageId")).Text;
            }
        }
    }
}
