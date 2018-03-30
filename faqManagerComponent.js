(function () {
    "use strict"; //can't have any variables we didn't define

    angular.module(AppName).component("faqManager", { //appname global var   we are attaching component to app name //takes in string and object
        bindings: {}, //
        templateUrl: "/scripts/components/views/faqManagerView.html",

        controller: function (requestService, $scope, $window, $uibModal) {
            var vm = this;

            vm.faqCategoryItem = []; //faq categories are populated in here
            vm.$onInit = _init;

            vm.faqModel = {}; //this is the model that holds the info from the form (the form {})
            vm.postFaq = _postFaq;//post funciton EVENTS

            vm.allFaqs = [];//empty array that i shove all the items into so i can show
            vm.getAllFaqs = _getAllFaqs;//show all faqs function

            vm.deleteFaq = _deleteFaq;//deleting the faq

            vm.populateFaqFormWithPostedFaq = _populateFaqFormWithPostedFaq; //edit. populates the form to edit

            vm.launchFaqCategoryModalWindow = _launchFaqCategoryModalWindow; //launches modal for creating new category

            vm.categoryAndFaqItemList = []; //this array has an array of object of the category as well as the item which is an array and which has an object in it. so like the object of "General" category and FAQs which is an array of objects with question, answer, display order and such

            
            function _init() {//calls the categories and gets all faq items
                requestService.ApiRequestService("get", "\/api/FaqCategories")
                    .then(function (response) {
                        vm.faqCategoryItem = response.items;
                        _getAllFaqs();
                    })
                    .catch(function (err) {
                        console.log(err);
                    })
            }

            function _postFaq(form) {//submit button that is a PUT button if there is an id
                if (!vm.faqModel.id) {//since no id its a POST
                    requestService.ApiRequestService("post", "\/api/FaqItems", vm.faqModel)
                        .then(function (response) {
                            swal({ title: "Done!", text: "FAQ was successfully added", timer: 1800, showConfirmButton: false, type: "success" });
                            vm.faqModel = {};
                            form.$setPristine();
                            form.$setUntouched();
                            _getAllFaqs();
                        })
                        .catch(function (err) {
                            swal({ title: "Opps!", text: "Something went wrong: " + err.data, timer: 1800, showConfirmButton: false, type: "error" });
                            console.log(err);
                        })
                } else {
                    //has an id so its PUT
                    requestService.ApiRequestService("put", "\/api/FaqItems/" + vm.faqModel.id, vm.faqModel)
                        .then(function (response) {
                            swal({ title: "Done!", text: "FAQ was successfully edited", timer: 1800, showConfirmButton: false, type: "success" });
                            vm.faqModel = {};
                            _getAllFaqs();
                        })
                        .catch(function (err) {
                            console.log(err);
                        })
                }

            }

            function _getAllFaqs() {//fills arry with items
                requestService.ApiRequestService("get", "\/api/FaqItems/CatItems")
                    .then(function (response) {
                        vm.allFaqs = [];
                        vm.allFaqs = response.items;
                        vm.categoryAndFaqItemList = [];

                         for (var i = 0; i < vm.faqCategoryItem.length; i++) {//on a loop, 
                            var categoryFaq = {};//empty object
                            categoryFaq.category = vm.faqCategoryItem[i].title;//.category prop = every cat item

                            var faqList = vm.allFaqs.filter(function (obj) {//.faqlist prop = filter the ones where the cat id === faqcat[i].id
                                return (obj.faqCategoryId === vm.faqCategoryItem[i].id)
                            })
                            categoryFaq.faqsList = faqList; //so now faqlist has all the items filerted with the same id==id
                            vm.categoryAndFaqItemList.push(categoryFaq);//push this var {} to an empty vm array
                        }
                    })
                    .catch(function (err) {
                        console.log(err);
                    })
            }

            function _deleteFaq(id) { //deletes item
                swal({
                    title: "Are you sure you want to delete this FAQ?",
                    type: "error",
                    showCancelButton: true,
                    confirmButtonColor: "#FF0000",
                    confirmButtonText: "Okay",
                    closeOnCancel: true,
                    closeOnConfirm: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            requestService.ApiRequestService("delete", "\/api/FaqItems/" + id)
                                .then(function (response) {
                                    vm.faqModel = {};
                                    vm.categoryAndFaqItemList = [];
                                    _getAllFaqs();

                                })
                                .catch(function (err) {
                                    console.log(err);
                                });
                        }
                    });
                
            }

            function _populateFaqFormWithPostedFaq(item) {//puts stuff back into the form
                vm.faqModel = item;
            }

            function _launchFaqCategoryModalWindow() {
                var modalInstance = $uibModal.open({
                    animation: vm.animationsEnabled,
                    component: 'faqManagerCategory',
                    size: 'md',
                });

                modalInstance.result.then(function () {
                    // this function only executes when the modal is closed
                    _init();
                    vm.allFaqs = [];

                }, function () {
                    // this function only executes when the modal is dismissed
                    _init();
                    vm.allFaqs = [];
                });

            }
            
        }
    })
})();
