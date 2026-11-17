// Jakar.Extensions :: Jakar.Users.Tests

namespace Jakar.Extensions.Tests;


[TestFixture]
[TestOf(typeof(LoginRequest))]
public class LoginRequest_Tests : Assert
{
    // ─── LoginRequest must bind from a raw request body ───────────────────────

    [Test] public void LoginRequest_BindsRawBody()
    {
        const string BODY = """{"userLogin":"tyler","UserPassword":"hunter2"}""";

        LoginRequest back = LoginRequest.FromJson(BODY);
        this.AreEqual("tyler",   back.UserLogin);
        this.AreEqual("hunter2", back.UserPassword);
    }

    [Test] public void LoginRequest_JakarJsonRoundTrip()
    {
        LoginRequest  request = new("tyler", "hunter2");
        string        json    = request.ToJson();
        LoginRequest? back    = LoginRequest.FromJson(json);
        this.NotNull(back);
        this.AreEqual("tyler",   back!.UserLogin);
        this.AreEqual("hunter2", back.UserPassword);
    }


    [Test] public void LoginRequestValue_CarriesAnyJson()
    {
        LoginRequestValue request = new("tyler", "hunter2", JNode.Parse("{\"Token\":\"abc\",\"Ttl\":30}"));
        LoginRequestValue back    = LoginRequestValue.FromJson(request.ToJson());

        Assert.That(back.UserPassword,                             Is.EqualTo("hunter2"));
        string? token = (string?)( (JObjectNode)back.Data! )["Token"];
        Assert.That(token,                                         Is.Not.Null);
        Assert.That(token,                                         Is.EqualTo("abc"));
        Assert.That(new LoginRequestValue().ToJson(),              Does.Contain("\"Data\" : null"));
    }


    [Test] public void LoginRequest_CopyConstructors_KeepThePassword()
    {
        LoginRequest        source  = new("tyler", "hunter2");
        LoginRequestVersion version = new(new LoginRequestVersion(source, AppVersion.Default));
        Assert.That(version.UserPassword,                                                                    Is.EqualTo("hunter2"));
        Assert.That(version.Equals((object)new LoginRequestVersion("tyler", "hunter2", AppVersion.Default)), Is.True);
    }

}