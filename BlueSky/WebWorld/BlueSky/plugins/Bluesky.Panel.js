/*
*
* Bluesky Components Panel Library v1.0
* 
* Copyright 2014, Yihuan Hu
*
*/
(function(Bluesky) {
    if (Bluesky && Bluesky.component) {
        Bluesky.extend(false, Bluesky.component, { Panel: function() {
            var args = arguments[0];
            return Bluesky.extend(true, {}, this, args);
        }
        });
    }
})(Bluesky);