namespace VetClinic.Core.Permissions
{
    public class ApplicationPermission
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }

        public ApplicationPermission()
        { }

        public ApplicationPermission(string name, string value, string groupName, string description)
        {
            Name = name;
            Value = value;
            GroupName = groupName;
            Description = description;
        }

        public override string ToString()
        {
            return Value;
        }


        public static implicit operator string(ApplicationPermission permission)
        {
            return permission.Value;
        }
    }
}
