﻿namespace Interns.Auth.Attributes.HasRole
{
    public class CalledByStaff() : HasRoleAttribute(UserRoles.ADMIN, UserRoles.SCHOOL_REPRESENTATIVE) { }
    public class CalledByStudent() : HasRoleAttribute(UserRoles.STUDENT) { }

    public class CalledWithRole() : HasRoleAttribute(UserRoles.ADMIN, UserRoles.SCHOOL_REPRESENTATIVE, UserRoles.STUDENT) { }
}
