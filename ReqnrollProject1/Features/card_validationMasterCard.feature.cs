﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace IntegrationTests.Features
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class MasterCardCreditCardValidationFeature
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private static string[] featureTags = ((string[])(null));
        
        private static global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "MasterCard Credit Card Validation", ("  As an API consumer\r\n  I want to validate credit card details\r\n  So that only va" +
                "lid credit card transactions are processed"), global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
        
#line 1 "card_validationMasterCard.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static async System.Threading.Tasks.Task FeatureSetupAsync(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute(Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupBehavior.EndOfClass)]
        public static async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(featureHint: featureInfo);
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Equals(featureInfo) == false)))
            {
                await testRunner.OnFeatureEndAsync();
            }
            if ((testRunner.FeatureContext == null))
            {
                await testRunner.OnFeatureStartAsync(featureInfo);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
            global::Reqnroll.TestRunnerManager.ReleaseTestRunner(testRunner);
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        public virtual async System.Threading.Tasks.Task FeatureBackgroundAsync()
        {
#line 6
#line hidden
#line 7
 await testRunner.GivenAsync("the API base URL is set", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Validate MasterCard owner field")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "MasterCard Credit Card Validation")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master", "200", "20", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("MASTER", "200", "20", "5555555555554123", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("master", "200", "20", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Doe", "200", "20", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Mary Lee", "200", "20", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Doe Smith Brown", "400", "\"Wrong owner\"", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master123", "400", "\"Wrong owner\"", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master_Doe", "400", "\"Wrong owner\"", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master@Smith", "400", "\"Wrong owner\"", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("\"   Master Doe\"", "400", "\"Wrong owner\"", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("\"Master Doe   \"", "400", "\"Wrong owner\"", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("\"12134 442\"", "400", "\"Wrong owner\"", "5555555555555444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("\"\"", "400", "\"Wrong owner\"", "5555555555555444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master  Doe", "400", "\"Wrong owner\"", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("O\'Master", "400", "\"Wrong owner\"", "5555555555554444", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master\\tBob", "400", "\"Wrong owner\"", "5555555555554444", null)]
        public async System.Threading.Tasks.Task ValidateMasterCardOwnerField(string owner, string statusCode, string expectedMessage, string number, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("owner", owner);
            argumentsOfScenario.Add("statusCode", statusCode);
            argumentsOfScenario.Add("expectedMessage", expectedMessage);
            argumentsOfScenario.Add("number", number);
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Validate MasterCard owner field", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 10
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 6
await this.FeatureBackgroundAsync();
#line hidden
#line 11
    await testRunner.GivenAsync(string.Format("a credit card with ONLY owner \"{0}\" and number \"{1}\"", owner, number), ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 12
    await testRunner.WhenAsync("I send a POST request to \"/CardValidation/card/credit/validate\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 13
    await testRunner.ThenAsync(string.Format("the response status should be {0}", statusCode), ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 14
    await testRunner.AndAsync(string.Format("the response should contain an error for Owner \"{0}\"", expectedMessage), ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Validate MasterCard card number fields")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "MasterCard Credit Card Validation")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Johnson", "5111111111111111", "200", "20", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Smith", "5212345678901234", "200", "20", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Brown", "5312345678901234", "200", "20", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Taylor", "5412345678901234", "200", "20", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Lee", "5512345678901234", "200", "20", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master White", "2221001234567890", "200", "20", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Garcia", "2720999999999999", "200", "20", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Clark", "511111111111111", "400", "\"Wrong number\"", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Davis", "51111111111111111", "400", "\"Wrong number\"", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Lopez", "22210012345678901", "400", "\"Wrong number\"", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Adams", "53123456789012", "400", "\"Wrong number\"", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Thompson", "5111a11111111111", "400", "\"Wrong number\"", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Miller", "53123#5678901234", "400", "\"Wrong number\"", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Brown", "aaaaaaaaaaaaaaaa", "400", "\"Wrong number\"", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Lopez", "27209999999999aa", "400", "\"Wrong number\"", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Brown", "5111-1111-1111-1111", "400", "\"Wrong number\"", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("Master Lee", "5312 3456 7890 1234", "400", "\"Wrong number\"", null)]
        public async System.Threading.Tasks.Task ValidateMasterCardCardNumberFields(string owner, string number, string statusCode, string expectedMessage, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("owner", owner);
            argumentsOfScenario.Add("number", number);
            argumentsOfScenario.Add("statusCode", statusCode);
            argumentsOfScenario.Add("expectedMessage", expectedMessage);
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Validate MasterCard card number fields", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 38
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 6
await this.FeatureBackgroundAsync();
#line hidden
#line 39
    await testRunner.GivenAsync(string.Format("a credit card with ONLY owner \"{0}\" and number \"{1}\"", owner, number), ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 40
 await testRunner.WhenAsync("I send a POST request to \"/CardValidation/card/credit/validate\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 41
 await testRunner.ThenAsync(string.Format("the response status should be {0}", statusCode), ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 42
 await testRunner.AndAsync(string.Format("the response should contain an error for Number \"{0}\"", expectedMessage), ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Validate a valid MasterCard credit card")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "MasterCard Credit Card Validation")]
        public async System.Threading.Tasks.Task ValidateAValidMasterCardCreditCard()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Validate a valid MasterCard credit card", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 76
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 6
await this.FeatureBackgroundAsync();
#line hidden
#line 77
 await testRunner.GivenAsync("a valid MasterCard credit card", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 78
 await testRunner.WhenAsync("I send a POST request to \"/CardValidation/card/credit/validate\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 79
 await testRunner.ThenAsync("the response status should be 200", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 80
 await testRunner.AndAsync("the response should contain \"20\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Validate a MasterCard with incorrect CVC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "MasterCard Credit Card Validation")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("12", "", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("12345", "", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("abc", "", null)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DataRowAttribute("12a", "", null)]
        public async System.Threading.Tasks.Task ValidateAMasterCardWithIncorrectCVC(string cvvNumber, string notUsed6248, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("cvvNumber", cvvNumber);
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Validate a MasterCard with incorrect CVC", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 82
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 6
await this.FeatureBackgroundAsync();
#line hidden
#line 83
 await testRunner.GivenAsync(string.Format("a valid MasterCard with an incorrect CVC \"{0}\"", cvvNumber), ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 84
 await testRunner.WhenAsync("I send a POST request to \"/CardValidation/card/credit/validate\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 85
 await testRunner.ThenAsync("the response status should be 400", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 86
 await testRunner.AndAsync("the response should contain \"Wrong cvv\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
