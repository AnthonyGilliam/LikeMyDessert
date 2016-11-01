var $lmd = {
    TextIsSignificant: function (text, minimumCharacters) {
        var pattern = new RegExp(/\w/mg);
        if (pattern.test(text) & text.length >= minimumCharacters)
            return true;

        return false;
    },
    HasCussing: function (text) {
        var cussWords = ['shit', 'fuck', 'dick', 'bitch', 'bastard', 'nigger', 'faggot'];

        for (var i = 0; cussWords.length; i++) {
            if (text.contains(cussWords[i]))
                return false;
        }

        return true;
    }
}