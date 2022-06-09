namespace WoodenWorkshop.Invitations.Core.Errors;

public static class ErrorMessages
{
    public const string InvitationNotFound = "Приглашение не найдено.";
    public const string UserEmailAddressIsInUse = "Адрес электронной почты уже использууется другим пользователем.";
    public const string InvitationInactive = "Приглашение не акутально или уже было принято.";
    public const string ActivateInvitationAttempt = "Приглашение не может быть активировано заново. Вместо этого необходимо выслать новое.";
}