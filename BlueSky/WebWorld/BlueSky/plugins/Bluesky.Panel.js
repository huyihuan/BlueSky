/**********************************************
* Bluesky框架组件库 Panel组件
* @copyright huyihuan 2013
* Date: 2013-11-12 20:31:11
**********************************************/
if (Bluesky && Bluesky.component) {
    Bluesky.extend(false, Bluesky.component, { Panel: function() {
        var args = arguments[0];
        return Bluesky.extend(true, {}, this, args);
    }
    });
}