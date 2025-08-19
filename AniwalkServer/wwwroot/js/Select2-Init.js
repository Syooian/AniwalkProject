//Select2 初始化
console.log("Select2 Init");
try {
    $(document).ready(function () {
        $('.js-example-basic-single').select2();
        console.log("Select2 initialized.");
    });
}
catch (E) {
    console.error("Select2 initialization failed : ", E);
}