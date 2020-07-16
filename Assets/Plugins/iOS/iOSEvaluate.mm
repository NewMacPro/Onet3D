#import <StoreKit/StoreKit.h>

extern "C" void EvaluateOc(){
    /**
     * 只能评分，不能编写评论
     * 有次数限制，一年只能使用三次
     * 使用次数超限后，需要跳转appstore
     */
    if([SKStoreReviewController respondsToSelector:@selector(requestReview)]) {// iOS 10.3 以上支持
        //防止键盘遮挡
        [[UIApplication sharedApplication].keyWindow endEditing:YES];
        [SKStoreReviewController requestReview];
    }
}