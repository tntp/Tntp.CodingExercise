
QUnit.test( "Sign in (username, password) required to post.", function( assert ) {
  assert.expect(4);
  var done1 = assert.async();
  var done2 = assert.async();
  var done3 = assert.async();
  var done4 = assert.async();
  myUsername = "JM";
  myPwd = "jm1";
  AddComment(function(result) {
    assert.ok(!result);
    done1();
  });
    SignIn(function(result)
    {
      assert.ok(result);
      done2();
      document.getElementById("commentText").value = "example comment text";
      AddComment(function(result) {
        assert.ok(result);
        done3();
        SignOut(function(result) {
          assert.ok(result);
          done4();
        });
      });
    });
});

QUnit.test( "Display name and message required to post.", function( assert ) {
  assert.expect(3);
  var done1 = assert.async();
  var done2 = assert.async();
  var done3 = assert.async();
  myUsername = "JM";
  myPwd = "jm1";
  SignIn(function(result)
    {
      assert.ok(result);
      done1();
      document.getElementById("commentText").value = "";
      AddComment(function(result) {
        assert.ok(!result);
        done2();
        SignOut(function(result) {
          assert.ok(result);
          done3();
        });
      });
    });
});

QUnit.test( "140 characters or under required to post", function( assert ) {
  assert.expect(3);
  var done1 = assert.async();
  var done2 = assert.async();
  var done3 = assert.async();
  myUsername = "JM";
  myPwd = "jm1";
  SignIn(function(result)
    {
      assert.ok(result);
      done1();
      document.getElementById("commentText").value = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
      AddComment(function(result) {
        assert.ok(!result);
        done2();
        SignOut(function(result) {
          assert.ok(result);
          done3();
        });
      });
    });
});

QUnit.test("Basic SignIn", function( assert ) {
  var done = assert.async();
  //Load the username and password.
  myUsername = "JM";
  myPwd = "jm1";
  SignIn(function(result)
    {
      assert.ok(result);
      done();
    });
});
QUnit.test("Basic SignOut", function( assert ) {
  var done = assert.async();
  SignOut(function(result)
    {
      assert.ok(result);
      done();
    });
});
QUnit.test("Basic CreateNewUser (success very first time, failure afterwards).", function( assert ) {
  var done = assert.async();
  //Load the username and password and display name.
  document.getElementById("newUsername").value = "testUser1";
  document.getElementById("newPwd").value = "testPwd1";
  document.getElementById("newDisplayName").value = "testDispName1";
  CreateNewUser(function(result)
    {
      assert.ok(!result);
      done();
    });
});