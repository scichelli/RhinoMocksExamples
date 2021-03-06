using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;
using Should.Extensions.AssertExtensions;

namespace RhinoMocksExamples
{
    public class Arguments
    {
        private IGameResultsService _stub;
        private IGameResultsService _mock;

        [SetUp]
        public void SetUp()
        {
            _stub = MockRepository.GenerateStub<IGameResultsService>();
            _mock = MockRepository.GenerateMock<IGameResultsService>();
        }

        [Test]
        public void You_can_specify_an_argument_when_stubbing_behavior()
        {
            const int intForFoo = 5;
            _stub.Stub(s => s.GetMagicNumber("foo")).Return(intForFoo);

            _stub.GetMagicNumber("foo").ShouldEqual(intForFoo);
            _stub.GetMagicNumber("bar").ShouldEqual(0);
        }

        [Test]
        public void Use_Is_Anything_when_args_are_not_important()
        {
            const int intToReturn = 5;
            _stub.Stub(s => s.GetMagicNumber(Arg<string>.Is.Anything))
                .Return(intToReturn);

            _stub.GetMagicNumber("foo").ShouldEqual(intToReturn);
            _stub.GetMagicNumber("bar").ShouldEqual(intToReturn);
            _stub.GetMagicNumber(null).ShouldEqual(intToReturn);
        }

        [Test]
        public void Use_Matches_to_define_conditions_for_args()
        {
            const int intForLongerStrings = 5;
            _stub.Stub(s =>
                      s.GetMagicNumber(
                          Arg<string>.Matches(arg => arg != null && arg.Length > 2)))
                .Return(intForLongerStrings);

            _stub.GetMagicNumber("fooo").ShouldEqual(intForLongerStrings);
            _stub.GetMagicNumber("foo").ShouldEqual(intForLongerStrings);
            _stub.GetMagicNumber("fo").ShouldEqual(0);
            _stub.GetMagicNumber("f").ShouldEqual(0);
            _stub.GetMagicNumber(null).ShouldEqual(0);
        }

        [Test]
        public void Use_GetArgumentsForCallsMadeOn_to_inspect_arguments()
        {
            var game = new Game();

            game.GetMagicNumberTwice(_mock);

            IList<object[]> argsPerCall =
                _mock.GetArgumentsForCallsMadeOn(m => m.GetMagicNumber(null));
            argsPerCall[0][0].ShouldEqual("foo");
            argsPerCall[1][0].ShouldEqual("bar");
        }

        [Test]
        public void Use_List_OneOf_to_test_if_arg_is_in_a_list()
        {
            var permittedValues = new[] { "hello", "hi", "o hai" };

            _mock.GetMagicNumber("hi");

            _mock.AssertWasCalled(
                m => m.GetMagicNumber(
                    Arg<string>.List.OneOf(permittedValues)));
        }
    }
}















